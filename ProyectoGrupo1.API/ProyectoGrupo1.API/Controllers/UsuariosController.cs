using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Api.DTOs;
using ProyectoGrupo1.Api.Models;
using ProyectoGrupo1.Api.Services;

namespace ProyectoGrupo1.Api.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _svc;
        public UsuariosController(UsuarioService svc) => _svc = svc;

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PerfilDto>> Get(int id)
        {
            var p = await _svc.ObtenerPerfilCompletoAsync(id);
            if (p is null) return NotFound();
            return Ok(new PerfilDto(
                p.UsuarioID, p.Nombre, p.Apellido, p.Correo,
                p.Direccion, p.Ciudad, p.Provincia, p.CodigoPostal, p.RolID));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] PerfilUpdateDto dto)
        {
            if (id != dto.UsuarioID) return BadRequest(new { error = "ID no coincide" });

            var ok = await _svc.ActualizarPerfilYDireccionAsync(new Usuario
            {
                UsuarioID = dto.UsuarioID,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Direccion = dto.Direccion,
                Ciudad = dto.Ciudad,
                Provincia = dto.Provincia,
                CodigoPostal = dto.CodigoPostal
            });
            return ok ? NoContent() : BadRequest(new { error = "No se actualizó" });
        }

        [HttpPut("{id:int}/password")]
        public async Task<ActionResult> ChangePassword(int id, [FromBody] PasswordChangeDto dto)
        {
            var ok = await _svc.CambiarContrasenaAsync(id, dto.ContrasenaActual, dto.NuevaContrasena);
            if (!ok) return BadRequest(new { error = "La contraseña actual no es válida" });
            return NoContent();
        }
    }
}
