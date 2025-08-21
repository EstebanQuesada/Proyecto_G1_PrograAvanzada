
using System.Net.Http.Json;
using ProyectoGrupo1.Models;

namespace ProyectoGrupo1.Services
{
    public interface IContactoApiClient
    {
        Task CrearAsync(ContactoViewModel vm, CancellationToken ct = default);
    }

    public class ContactoApiClient : IContactoApiClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<ContactoApiClient> _logger;

        public ContactoApiClient(HttpClient http, ILogger<ContactoApiClient> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task CrearAsync(ContactoViewModel vm, CancellationToken ct = default)
        {
            var dto = new { nombre = vm.Nombre, correo = vm.Correo, mensaje = vm.Mensaje };
            var resp = await _http.PostAsJsonAsync("api/v1/contactos", dto, ct);

            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(ct);
                _logger.LogWarning("Error API creando contacto: {Status} {Body}", (int)resp.StatusCode, body);
                throw new InvalidOperationException("No se pudo enviar el mensaje en este momento.");
            }
        }
    }
}

