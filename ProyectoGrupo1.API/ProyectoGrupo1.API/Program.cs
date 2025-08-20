
using ProyectoGrupo1.Api.Infra;    
using ProyectoGrupo1.Api.Services;
using ProyectoGrupo1.Api.Repositories;
using Microsoft.OpenApi.Models;
using ProyectoGrupo1.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbConnectionFactory>(sp =>
    new SqlConnectionFactory(
        builder.Configuration.GetConnectionString("TiendaRopaDB")
        ?? throw new Exception("ConnectionStrings:TiendaRopaDB no configurado")));

builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<AdminUsuarioService>();


builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ProyectoGrupo1 API", Version = "v1" });
});

builder.Services.AddCors(o => o.AddPolicy("AppCors", p =>
    p.WithOrigins("https://localhost:5001", "http://localhost:5000") 
     .AllowAnyHeader()
     .AllowAnyMethod()
));

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var err = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        await context.Response.WriteAsync($"{{\"error\":\"{err?.Message}\"}}");
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AppCors");      
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
