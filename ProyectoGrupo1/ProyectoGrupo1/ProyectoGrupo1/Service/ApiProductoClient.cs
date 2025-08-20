using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using ProyectoGrupo1.Models;
using System.Globalization;

public class ApiProductoClient
{
    private readonly HttpClient _http;
    public ApiProductoClient(HttpClient http) => _http = http;

    private static string BuildUrl(string path, IDictionary<string, string?> qs)
        => QueryHelpers.AddQueryString(path, qs.Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                                              .ToDictionary(kv => kv.Key, kv => kv.Value));

    public async Task<List<ProductoCatalogoViewModel>> CatalogoAsync(
        string? busqueda, string? categoria, decimal? precioMin, decimal? precioMax,
        CancellationToken ct = default)
    {
        var url = BuildUrl("api/productos/catalogo", new Dictionary<string, string?>
        {
            ["busqueda"] = busqueda,
            ["categoria"] = categoria,
            ["precioMin"] = precioMin?.ToString(CultureInfo.InvariantCulture),
            ["precioMax"] = precioMax?.ToString(CultureInfo.InvariantCulture)
        });

        var resp = await _http.GetAsync(url, ct);
        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            throw new ApiClientException("No se pudo cargar el catálogo.", resp.StatusCode, body);
        }
        return await resp.Content.ReadFromJsonAsync<List<ProductoCatalogoViewModel>>(cancellationToken: ct) ?? new();
    }

    public async Task<List<string>> CategoriasAsync(CancellationToken ct = default)
    {
        var resp = await _http.GetAsync("api/productos/categorias", ct);
        if (!resp.IsSuccessStatusCode)
            throw new ApiClientException("No se pudieron cargar las categorías.", resp.StatusCode);
        return await resp.Content.ReadFromJsonAsync<List<string>>(cancellationToken: ct) ?? new();
    }

    public async Task<ProductoDetalleViewModel?> DetalleAsync(int id, CancellationToken ct = default)
    {
        var resp = await _http.GetAsync($"api/productos/{id}", ct);
        if (resp.StatusCode == HttpStatusCode.NotFound) return null;
        if (!resp.IsSuccessStatusCode)
            throw new ApiClientException("No se pudo obtener el detalle.", resp.StatusCode);
        return await resp.Content.ReadFromJsonAsync<ProductoDetalleViewModel>(cancellationToken: ct);
    }

    public async Task<ProductoTallaColor?> PtcAsync(int ptcId, CancellationToken ct = default)
    {
        var resp = await _http.GetAsync($"api/productos/ptc/{ptcId}", ct);
        if (resp.StatusCode == HttpStatusCode.NotFound) return null;
        if (!resp.IsSuccessStatusCode)
        {
            var msg = await resp.Content.ReadAsStringAsync(ct);
            throw new ApiClientException($"No se pudo consultar la combinación.", resp.StatusCode, msg);
        }
        return await resp.Content.ReadFromJsonAsync<ProductoTallaColor>(cancellationToken: ct);
    }
}