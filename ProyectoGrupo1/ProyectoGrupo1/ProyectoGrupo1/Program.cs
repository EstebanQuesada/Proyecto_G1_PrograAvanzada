using ProyectoGrupo1.Service;
using ProyectoGrupo1.Services; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<CarritoService>();
builder.Services.AddScoped<DbService>();
builder.Services.AddScoped<TextoService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(60);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

var apiBase = builder.Configuration["Api:BaseUrl"]
    ?? throw new InvalidOperationException("Falta configurar 'Api:BaseUrl' en appsettings.json");
if (!apiBase.EndsWith("/")) apiBase += "/";
var apiUri = new Uri(apiBase, UriKind.Absolute);

builder.Services.AddHttpClient<ApiProductoClient>(c =>
{
    c.BaseAddress = apiUri;
    c.Timeout = TimeSpan.FromSeconds(10);
    c.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<ApiAdminContactoClient>(c =>
{
    c.BaseAddress = apiUri;
    c.Timeout = TimeSpan.FromSeconds(10);
    c.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<ApiUsuarioClient>(c =>
{
    c.BaseAddress = apiUri;
    c.Timeout = TimeSpan.FromSeconds(10);
    c.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<ApiAdminUsuarioClient>(c =>
{
    c.BaseAddress = apiUri;
    c.Timeout = TimeSpan.FromSeconds(10);
    c.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<ApiAdminProductoClient>(c =>
{
    c.BaseAddress = apiUri;
    c.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<PedidoApiService>(c =>
{
    c.BaseAddress = apiUri;
    c.Timeout = TimeSpan.FromSeconds(15);
    c.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<ApiCatalogosClient>(c =>
{
    c.BaseAddress = apiUri;
    c.Timeout = TimeSpan.FromSeconds(15);
    c.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<IContactoApiClient, ContactoApiClient>(c =>
{
    c.BaseAddress = apiUri;
    c.Timeout = TimeSpan.FromSeconds(10);
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

app.UseAuthentication();   
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
