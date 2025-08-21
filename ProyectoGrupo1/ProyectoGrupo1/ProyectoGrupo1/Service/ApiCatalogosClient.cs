using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ProyectoGrupo1.Service
{
    public class ApiCatalogosClient
    {
        private readonly HttpClient _http;
        public ApiCatalogosClient(HttpClient http) => _http = http;

        public record LookupVm(int Id, string Nombre);

        public record ProveedorVm(
            int Id,
            [property: JsonPropertyName("NombreProveedor")] string Nombre,
            string Correo,
            string Telefono
        );

        public record ProveedorSaveVm(
            [property: JsonPropertyName("NombreProveedor")] string Nombre,
            string Correo,
            string Telefono
        );

        public Task<List<LookupVm>?> CategoriasAsync()
            => _http.GetFromJsonAsync<List<LookupVm>>("api/v1/admin/catalogos/categorias");
        public async Task<bool> CrearCategoriaAsync(string nombre)
            => (await _http.PostAsJsonAsync("api/v1/admin/catalogos/categorias", new { Id = 0, Nombre = nombre })).IsSuccessStatusCode;
        public async Task<bool> ActualizarCategoriaAsync(int id, string nombre)
            => (await _http.PutAsJsonAsync($"api/v1/admin/catalogos/categorias/{id}", new { Id = id, Nombre = nombre })).IsSuccessStatusCode;
        public async Task<bool> EliminarCategoriaAsync(int id)
            => (await _http.DeleteAsync($"api/v1/admin/catalogos/categorias/{id}")).IsSuccessStatusCode;

        public Task<List<LookupVm>?> MarcasAsync()
            => _http.GetFromJsonAsync<List<LookupVm>>("api/v1/admin/catalogos/marcas");
        public async Task<bool> CrearMarcaAsync(string nombre)
            => (await _http.PostAsJsonAsync("api/v1/admin/catalogos/marcas", new { Id = 0, Nombre = nombre })).IsSuccessStatusCode;
        public async Task<bool> ActualizarMarcaAsync(int id, string nombre)
            => (await _http.PutAsJsonAsync($"api/v1/admin/catalogos/marcas/{id}", new { Id = id, Nombre = nombre })).IsSuccessStatusCode;
        public async Task<bool> EliminarMarcaAsync(int id)
            => (await _http.DeleteAsync($"api/v1/admin/catalogos/marcas/{id}")).IsSuccessStatusCode;

        public Task<List<LookupVm>?> TallasAsync()
            => _http.GetFromJsonAsync<List<LookupVm>>("api/v1/admin/catalogos/tallas");
        public async Task<bool> CrearTallaAsync(string nombre)
            => (await _http.PostAsJsonAsync("api/v1/admin/catalogos/tallas", new { Id = 0, Nombre = nombre })).IsSuccessStatusCode;
        public async Task<bool> ActualizarTallaAsync(int id, string nombre)
            => (await _http.PutAsJsonAsync($"api/v1/admin/catalogos/tallas/{id}", new { Id = id, Nombre = nombre })).IsSuccessStatusCode;
        public async Task<bool> EliminarTallaAsync(int id)
            => (await _http.DeleteAsync($"api/v1/admin/catalogos/tallas/{id}")).IsSuccessStatusCode;

        public Task<List<LookupVm>?> ColoresAsync()
            => _http.GetFromJsonAsync<List<LookupVm>>("api/v1/admin/catalogos/colores");
        public async Task<bool> CrearColorAsync(string nombre)
            => (await _http.PostAsJsonAsync("api/v1/admin/catalogos/colores", new { Id = 0, Nombre = nombre })).IsSuccessStatusCode;
        public async Task<bool> ActualizarColorAsync(int id, string nombre)
            => (await _http.PutAsJsonAsync($"api/v1/admin/catalogos/colores/{id}", new { Id = id, Nombre = nombre })).IsSuccessStatusCode;
        public async Task<bool> EliminarColorAsync(int id)
            => (await _http.DeleteAsync($"api/v1/admin/catalogos/colores/{id}")).IsSuccessStatusCode;

        public Task<List<ProveedorVm>?> ProveedoresAsync()
            => _http.GetFromJsonAsync<List<ProveedorVm>>("api/v1/admin/catalogos/proveedores");
        public async Task<bool> CrearProveedorAsync(ProveedorSaveVm dto)
            => (await _http.PostAsJsonAsync("api/v1/admin/catalogos/proveedores", dto)).IsSuccessStatusCode;
        public async Task<bool> ActualizarProveedorAsync(int id, ProveedorSaveVm dto)
            => (await _http.PutAsJsonAsync($"api/v1/admin/catalogos/proveedores/{id}", dto)).IsSuccessStatusCode;
        public async Task<bool> EliminarProveedorAsync(int id)
            => (await _http.DeleteAsync($"api/v1/admin/catalogos/proveedores/{id}")).IsSuccessStatusCode;
    }
}
