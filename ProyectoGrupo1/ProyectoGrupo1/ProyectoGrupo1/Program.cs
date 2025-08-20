using ProyectoGrupo1.Service;
using ProyectoGrupo1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<CarritoService>();
builder.Services.AddSingleton<ProyectoGrupo1.Services.DbService>(); 
builder.Services.AddScoped<TextoService>();
builder.Services.AddScoped<ContactoService>();
builder.Services.AddScoped<PedidoApiService>();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(60);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

var apiBase = builder.Configuration["Api:BaseUrl"]
              ?? throw new Exception("Falta configurar 'Api:BaseUrl' en appsettings.json");

builder.Services.AddHttpClient<ApiProductoClient>(c =>
{
    c.BaseAddress = new Uri(apiBase);
    c.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<ApiUsuarioClient>(c =>
{
    c.BaseAddress = new Uri(apiBase);
    c.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<ApiAdminUsuarioClient>(c =>
{
    c.BaseAddress = new Uri(apiBase);
    c.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
