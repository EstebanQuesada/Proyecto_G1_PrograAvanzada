using ProyectoGrupo1.Service;
using ProyectoGrupo1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ProductoService>();
builder.Services.AddSingleton<ProyectoGrupo1.Services.DbService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<TextoService>();
builder.Services.AddScoped<ContactoService>();

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
