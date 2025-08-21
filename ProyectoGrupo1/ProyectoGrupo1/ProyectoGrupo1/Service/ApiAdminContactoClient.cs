
using System.Net.Http.Json;

namespace ProyectoGrupo1.Services
{
    public class AdminContactoItemVm
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public DateTime FechaEnvio { get; set; }
        public bool Visto { get; set; }
        public bool Completado { get; set; }
        public DateTime? FechaVisto { get; set; }
        public DateTime? FechaCompletado { get; set; }
    }

    public class AdminContactoListVm
    {
        public int Total { get; set; }
        public List<AdminContactoItemVm> Items { get; set; } = new();
    }

    public class ApiAdminContactoClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<ApiAdminContactoClient> _logger;

        public ApiAdminContactoClient(HttpClient http, ILogger<ApiAdminContactoClient> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<AdminContactoListVm> ListarAsync(string? buscar, string? estado, int page, int pageSize, CancellationToken ct = default)
        {
            var url = $"api/v1/contactos/admin?buscar={Uri.EscapeDataString(buscar ?? "")}&estado={Uri.EscapeDataString(estado ?? "")}&page={page}&pageSize={pageSize}";
            var resp = await _http.GetAsync(url, ct);
            resp.EnsureSuccessStatusCode();
            var anon = await resp.Content.ReadFromJsonAsync<AdminContactoListVm>(cancellationToken: ct)
                       ?? new AdminContactoListVm();
            return anon;
        }

        public async Task<bool> ActualizarEstadoAsync(int id, bool visto, bool completado, CancellationToken ct = default)
        {
            var payload = new { visto, completado };
            var resp = await _http.PutAsJsonAsync($"api/v1/contactos/admin/{id}/estado", payload, ct);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(ct);
                _logger.LogWarning("ActualizarEstadoAsync fallo: {Status} {Body}", (int)resp.StatusCode, body);
                return false;
            }
            return true;
        }
    }
}
