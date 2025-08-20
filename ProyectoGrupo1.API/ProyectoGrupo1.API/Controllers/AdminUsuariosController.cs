using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Api.Services;
using ProyectoGrupo1.Api.Models;

namespace ProyectoGrupo1.Api.Controllers;

[ApiController]
[Route("api/v1/admin/usuarios")]
public class AdminUsuariosController : ControllerBase
{
    private readonly AdminUsuarioService _svc;
    public AdminUsuariosController(AdminUsuarioService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<object>> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? q = null)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 100);

        var (total, items) = await _svc.ListarAsync(page, pageSize, q);
        return Ok(new { total, items });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Usuario>> Get(int id)
        => (await _svc.ObtenerAsync(id)) is { } u ? Ok(u) : NotFound();

    public record CrearDto(string Nombre, string Apellido, string Correo, string Contrasena,
                           int RolID, string Direccion, string Ciudad, string Provincia, string CodigoPostal);

    [HttpPost]
    public async Task<ActionResult> Crear([FromBody] CrearDto dto)
    {
        var id = await _svc.CrearAsync(new Usuario
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Correo = dto.Correo,
            Contrasena = dto.Contrasena,
            RolID = dto.RolID,
            Direccion = dto.Direccion,
            Ciudad = dto.Ciudad,
            Provincia = dto.Provincia,
            CodigoPostal = dto.CodigoPostal
        });
        return id > 0 ? StatusCode(201, new { id }) : BadRequest(new { error = "No se pudo crear" });
    }

    public record ActualizarDto(int UsuarioID, string Nombre, string Apellido, string Correo, int RolID,
                                string Direccion, string Ciudad, string Provincia, string CodigoPostal);

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Actualizar(int id, [FromBody] ActualizarDto dto)
        => await _svc.ActualizarAsync(new Usuario
        {
            UsuarioID = id,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Correo = dto.Correo,
            RolID = dto.RolID,
            Direccion = dto.Direccion,
            Ciudad = dto.Ciudad,
            Provincia = dto.Provincia,
            CodigoPostal = dto.CodigoPostal
        }) ? NoContent() : NotFound();

    public record RolDto(int RolID);

    [HttpPut("{id:int}/rol")]
    public async Task<ActionResult> CambiarRol(int id, [FromBody] RolDto dto)
        => await _svc.CambiarRolAsync(id, dto.RolID) ? NoContent() : NotFound();

    public record BloqueoDto(bool Bloqueado);

    [HttpPut("{id:int}/bloqueo")]
    public async Task<ActionResult> Bloquear(int id, [FromBody] BloqueoDto dto)
        => await _svc.BloquearAsync(id, dto.Bloqueado) ? NoContent() : NotFound();

    public record ResetPasswordDto(string NuevaContrasena);

    [HttpPut("{id:int}/reset-password")]
    public async Task<ActionResult> ResetPassword(int id, [FromBody] ResetPasswordDto dto)
        => await _svc.ResetPasswordAsync(id, dto.NuevaContrasena) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Eliminar(int id)
        => await _svc.EliminarAsync(id) ? NoContent() : NotFound();

    [HttpPut("{id:int}/restore")]
    public async Task<ActionResult> Restaurar(int id)
        => await _svc.RestaurarAsync(id) ? NoContent() : NotFound();
}
