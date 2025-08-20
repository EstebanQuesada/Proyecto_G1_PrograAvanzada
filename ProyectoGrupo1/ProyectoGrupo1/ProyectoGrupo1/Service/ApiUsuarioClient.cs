
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

public class ApiUsuarioClient
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    public ApiUsuarioClient(HttpClient http) => _http = http;

    public record PerfilVm(int UsuarioID, string Nombre, string Apellido, string Correo,
                           string Direccion, string Ciudad, string Provincia, string CodigoPostal, int RolID);

    public async Task<PerfilVm?> LoginAsync(string correo, string contrasena)
    {
        var res = await _http.PostAsJsonAsync("api/v1/auth/login", new { Correo = correo, Contrasena = contrasena });

        if (res.StatusCode == HttpStatusCode.Unauthorized) return null; 

        if (res.StatusCode == HttpStatusCode.Forbidden || (int)res.StatusCode == 423)
        {
            var body = await res.Content.ReadAsStringAsync();
            var msg = TryGetErrorMessage(body) ?? "No autorizado.";
            throw new ApplicationException(msg);
        }

        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PerfilVm>(json, _json);
    }

    public async Task<bool> RegisterAsync(object dto)
    {
        var res = await _http.PostAsJsonAsync("api/v1/auth/register", dto);
        return res.IsSuccessStatusCode;
    }

    public async Task<PerfilVm?> ObtenerPerfilAsync(int id)
    {
        var res = await _http.GetAsync($"api/v1/usuarios/{id}");
        if (res.StatusCode == HttpStatusCode.NotFound) return null;
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PerfilVm>(json, _json);
    }

    public async Task<bool> ActualizarPerfilAsync(object dto)
    {
        var id = (int)(dto.GetType().GetProperty("UsuarioID")!.GetValue(dto)
                       ?? throw new Exception("UsuarioID requerido"));
        var res = await _http.PutAsJsonAsync($"api/v1/usuarios/{id}", dto);
        return res.IsSuccessStatusCode;
    }

    public async Task<(bool ok, string? error)> CambiarPasswordAsync(int id, string actual, string nueva)
    {
        var res = await _http.PutAsJsonAsync($"api/v1/usuarios/{id}/password",
                                             new { ContrasenaActual = actual, NuevaContrasena = nueva });
        if (res.IsSuccessStatusCode) return (true, null);
        var text = await res.Content.ReadAsStringAsync();
        return (false, text);
    }

    private static string? TryGetErrorMessage(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("error", out var err) && err.ValueKind == JsonValueKind.String)
                return err.GetString();
        }
        catch { }
        return null;
    }
}

