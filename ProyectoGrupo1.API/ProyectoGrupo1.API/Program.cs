
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ProyectoGrupo1.API.Infra;
using ProyectoGrupo1.Api.Services;
using ProyectoGrupo1.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<AdminUsuarioService>();
builder.Services.AddScoped<IAdminProductoRepository, AdminProductoRepository>(); 
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ProyectoGrupo1 API", Version = "v1" });
});

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                      ?? new[] { "https://localhost:5001", "http://localhost:5000" };

builder.Services.AddCors(o => o.AddPolicy("AppCors", p =>
    p.WithOrigins(allowedOrigins)
     .AllowAnyHeader()
     .AllowAnyMethod()
));

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        var status = ex switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            InvalidOperationException ioe when ioe.Message == "STOCK_INSUFICIENTE" => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";

        var env = context.RequestServices.GetRequiredService<IHostEnvironment>();
        var problem = new ProblemDetails
        {
            Title = status == 500 ? "Error inesperado" : ex?.GetType().Name,
            Status = status,
            Detail = env.IsDevelopment() ? ex?.ToString() : "Se produjo un error procesando la solicitud.",
            Instance = context.TraceIdentifier
        };

        await context.Response.WriteAsJsonAsync(problem);
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AppCors");
app.UseHttpsRedirection();

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
