using Microsoft.AspNetCore.Mvc;

using System.Globalization;
using Microsoft.AspNetCore.WebUtilities;
using ProyectoGrupo1.Models;
using System.Net;
using static System.Net.WebRequestMethods;

public class ApiProductoClient
{
    private readonly HttpClient _http;
    public ApiProductoClient(HttpClient http) => _http = http;

   
    public async Task<List<ProductoCatalogoViewModel>> CatalogoAsync(
        string? busqueda, string? categoria, decimal? precioMin, decimal? precioMax)
    {
        var qs = new Dictionary<string, string?>
        {
            ["busqueda"] = busqueda,
            ["categoria"] = categoria,
            ["precioMin"] = precioMin?.ToString(CultureInfo.InvariantCulture),
            ["precioMax"] = precioMax?.ToString(CultureInfo.InvariantCulture)
        };
        var url = QueryHelpers.AddQueryString("api/productos/catalogo", qs!);

        var resp = await _http.GetAsync(url);
        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync();
            throw new ApplicationException($"API Catalogo {(int)resp.StatusCode}: {body}");
        }
        return (await resp.Content.ReadFromJsonAsync<List<ProductoCatalogoViewModel>>()) ?? new();
    }

    public async Task<List<string>> CategoriasAsync()
    {
        var resp = await _http.GetAsync("api/productos/categorias");
        if (!resp.IsSuccessStatusCode)
            throw new ApplicationException("No se pudieron cargar categorías.");
        return (await resp.Content.ReadFromJsonAsync<List<string>>()) ?? new();
    }

    public async Task<ProductoDetalleViewModel?> DetalleAsync(int id)
    {
        var resp = await _http.GetAsync($"api/productos/{id}");
        if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        if (!resp.IsSuccessStatusCode)
            throw new ApplicationException("No se pudo obtener el detalle.");
        return await resp.Content.ReadFromJsonAsync<ProductoDetalleViewModel>();
    }
    public async Task<ProductoTallaColor?> PtcAsync(int ptcId)
    {
        var resp = await _http.GetAsync($"api/productos/ptc/{ptcId}");
        if (resp.StatusCode == HttpStatusCode.NotFound) return null;
        if (!resp.IsSuccessStatusCode)
        {
            var msg = await resp.Content.ReadAsStringAsync();
            throw new ApplicationException($"No se pudo consultar el PTC {ptcId}. {msg}");
        }
        return await resp.Content.ReadFromJsonAsync<ProductoTallaColor>();
    }
}
