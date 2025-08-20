using System.Text;
using System.Text.Json;

public class ApiAdminUsuarioClient
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };
    public ApiAdminUsuarioClient(IHttpClientFactory f) => _http = f.CreateClient("Api");

    public record PagedResult<T>(int Total, IEnumerable<T> Items);
    public record AdminUserListItemDto(int UsuarioID, string Nombre, string Apellido, string Correo, int RolID, DateTime? FechaRegistro, bool Bloqueado, bool Activo);
    public record AdminUserDetailDto(int UsuarioID, string Nombre, string Apellido, string Correo, int RolID, string Direccion, string Ciudad, string Provincia, string CodigoPostal, bool Bloqueado, bool Activo);

    public Task<PagedResult<AdminUserListItemDto>?> ListarAsync(int page = 1, int pageSize = 10, string? q = null)
        => Get<PagedResult<AdminUserListItemDto>>($"/api/v1/admin/usuarios?page={page}&pageSize={pageSize}&q={Uri.EscapeDataString(q ?? "")}");

    public Task<AdminUserDetailDto?> ObtenerAsync(int id)
        => Get<AdminUserDetailDto>($"/api/v1/admin/usuarios/{id}");

    public Task<bool> CrearAsync(object dto)
        => Send(HttpMethod.Post, "/api/v1/admin/usuarios", dto);

    public Task<bool> ActualizarAsync(int id, object dto)
        => Send(HttpMethod.Put, $"/api/v1/admin/usuarios/{id}", dto);

    public Task<bool> CambiarRolAsync(int id, int rolId)
        => Send(HttpMethod.Put, $"/api/v1/admin/usuarios/{id}/rol", new { RolID = rolId });

    public Task<bool> BloquearAsync(int id, bool bloqueado)
        => Send(HttpMethod.Put, $"/api/v1/admin/usuarios/{id}/bloqueo", new { Bloqueado = bloqueado });

    public Task<bool> ResetPasswordAsync(int id, string nueva)
        => Send(HttpMethod.Put, $"/api/v1/admin/usuarios/{id}/reset-password", new { NuevaContrasena = nueva });

    public Task<bool> EliminarAsync(int id)
        => Send(HttpMethod.Delete, $"/api/v1/admin/usuarios/{id}", null);

    public Task<bool> RestaurarAsync(int id)
        => Send(HttpMethod.Put, $"/api/v1/admin/usuarios/{id}/restore", null);

    private async Task<T?> Get<T>(string url)
    {
        var res = await _http.GetAsync(url);
        if (!res.IsSuccessStatusCode) return default;
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, _json);
    }
    private async Task<bool> Send(HttpMethod m, string url, object? body)
    {
        var req = new HttpRequestMessage(m, url);
        if (body != null)
            req.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var res = await _http.SendAsync(req);
        return res.IsSuccessStatusCode;
    }
}
