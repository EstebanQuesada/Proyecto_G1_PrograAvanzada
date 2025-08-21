using System.Net.Http.Json;
using System.Text.Json;
using ProyectoGrupo1.Models;                         
using ProyectoGrupo1.Models.AdminProducto;         

namespace ProyectoGrupo1.Service
{
    public class ApiAdminProductoClient
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        public ApiAdminProductoClient(HttpClient http) => _http = http;

        public record LookupItemVm(int Id, string Nombre);
        public record AdminPtcVm(int? PTCID, int TallaID, int ColorID, int Stock, string? NombreTalla, string? NombreColor);

        public record AdminProductoSaveVm(
            string Nombre, string Descripcion, decimal Precio,
            int CategoriaID, int MarcaID, int ProveedorID,
            List<string> Imagenes, List<AdminPtcVm> PTCs
        );

        public record AdminProductoVm(
            int ProductoID, string Nombre, string Descripcion, decimal Precio,
            int CategoriaID, int MarcaID, int ProveedorID,
            List<string> Imagenes, List<AdminPtcVm> PTCs
        );

        public record AdminLookupsVm(
            List<LookupItemVm> Categorias,
            List<LookupItemVm> Marcas,
            List<LookupItemVm> Proveedores,
            List<LookupItemVm> Tallas,
            List<LookupItemVm> Colores
        );

        public record ObtenerResult(AdminProductoVm producto, AdminLookupsVm lookups);

        public async Task<ListResult<AdminProductoListItemVm>> ListarAsync(int page, int pageSize, string? q)
        {
            var url = $"api/v1/admin/productos?page={page}&pageSize={pageSize}&q={Uri.EscapeDataString(q ?? "")}";
            return await _http.GetFromJsonAsync<ListResult<AdminProductoListItemVm>>(url, _json)
            ?? new ListResult<AdminProductoListItemVm>
            {
             Total = 0,
             Items = Enumerable.Empty<AdminProductoListItemVm>()
            };
        }

        public async Task<ObtenerResult?> ObtenerAsync(int id)
        {
            var res = await _http.GetAsync($"api/v1/admin/productos/{id}");
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadFromJsonAsync<ObtenerResult>(_json);
        }


        public async Task<(bool ok, int? id, string? error)> CrearAsync(AdminProductoSaveVm dto, CancellationToken ct = default)
        {
            var res = await _http.PostAsJsonAsync("api/v1/admin/productos", dto, _json, ct);
            if (!res.IsSuccessStatusCode) return (false, null, await res.Content.ReadAsStringAsync(ct));
            var kv = await res.Content.ReadFromJsonAsync<Dictionary<string, int>>(cancellationToken: ct);
            return (true, kv is not null && kv.TryGetValue("productoId", out var id) ? id : null, null);
        }

        public async Task<(bool ok, string? error)> ActualizarAsync(int id, AdminProductoSaveVm dto, CancellationToken ct = default)
        {
            var res = await _http.PutAsJsonAsync($"api/v1/admin/productos/{id}", dto, _json, ct);
            return (res.IsSuccessStatusCode, res.IsSuccessStatusCode ? null : await res.Content.ReadAsStringAsync(ct));
        }

        public async Task<(bool ok, string? error)> EliminarAsync(int id, CancellationToken ct = default)
        {
            var res = await _http.DeleteAsync($"api/v1/admin/productos/{id}", ct);
            return (res.IsSuccessStatusCode, res.IsSuccessStatusCode ? null : await res.Content.ReadAsStringAsync(ct));
        }
    }
}
