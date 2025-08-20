using System.Text;
using System.Text.Json;

public class ApiUsuarioClient
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiUsuarioClient(IHttpClientFactory f) => _http = f.CreateClient("Api");

    public record PerfilVm(int UsuarioID, string Nombre, string Apellido, string Correo,
                           string Direccion, string Ciudad, string Provincia, string CodigoPostal, int RolID);

    public async Task<PerfilVm?> LoginAsync(string correo, string contrasena)
    {
        var payload = JsonSerializer.Serialize(new { Correo = correo, Contrasena = contrasena });
        var res = await _http.PostAsync("/api/v1/auth/login",
            new StringContent(payload, Encoding.UTF8, "application/json"));
        if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized) return null;
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PerfilVm>(json, _json);
    }

    public async Task<bool> RegisterAsync(object dto)
    {
        var payload = JsonSerializer.Serialize(dto);
        var res = await _http.PostAsync("/api/v1/auth/register",
            new StringContent(payload, Encoding.UTF8, "application/json"));
        return res.IsSuccessStatusCode;
    }

    public async Task<PerfilVm?> ObtenerPerfilAsync(int id)
    {
        var res = await _http.GetAsync($"/api/v1/usuarios/{id}");
        if (res.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PerfilVm>(json, _json);
    }

    public async Task<bool> ActualizarPerfilAsync(object dto)
    {
        var payload = JsonSerializer.Serialize(dto);
        var id = (int)(dto.GetType().GetProperty("UsuarioID")!.GetValue(dto)
                       ?? throw new Exception("UsuarioID requerido"));
        var res = await _http.PutAsync($"/api/v1/usuarios/{id}",
            new StringContent(payload, Encoding.UTF8, "application/json"));
        return res.IsSuccessStatusCode;
    }

    public async Task<(bool ok, string? error)> CambiarPasswordAsync(int id, string actual, string nueva)
    {
        var payload = JsonSerializer.Serialize(new { ContrasenaActual = actual, NuevaContrasena = nueva });
        var res = await _http.PutAsync($"/api/v1/usuarios/{id}/password",
            new StringContent(payload, Encoding.UTF8, "application/json"));
        if (res.IsSuccessStatusCode) return (true, null);
        var text = await res.Content.ReadAsStringAsync();
        return (false, text);
    }
}
