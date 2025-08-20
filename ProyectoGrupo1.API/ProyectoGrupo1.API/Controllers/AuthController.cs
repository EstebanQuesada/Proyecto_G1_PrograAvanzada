using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Api.DTOs;
using ProyectoGrupo1.Api.Models;
using ProyectoGrupo1.Api.Services;

namespace ProyectoGrupo1.Api.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _svc;
        public AuthController(UsuarioService svc) => _svc = svc;

        [HttpPost("login")]
        public async Task<ActionResult<PerfilDto>> Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = await _svc.ValidarUsuarioAsync(dto.Correo, dto.Contrasena);
                if (user is null)
                    return Unauthorized(new { error = "Credenciales incorrectas" });

                var perfil = await _svc.ObtenerPerfilCompletoAsync(user.UsuarioID);
                if (perfil is null)
                    return Unauthorized(new { error = "Usuario no encontrado" });

                return Ok(new PerfilDto(
                    perfil.UsuarioID, perfil.Nombre, perfil.Apellido, perfil.Correo,
                    perfil.Direccion, perfil.Ciudad, perfil.Provincia, perfil.CodigoPostal, perfil.RolID));
            }
            catch (AppException ex) when (ex.StatusCode == 403)
            {
                return StatusCode(403, new { error = ex.Message });
            }
            catch (AppException ex) when (ex.StatusCode == 423)
            {
                return StatusCode(423, new { error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            var nuevoId = await _svc.RegistrarUsuarioAsync(new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Contrasena = dto.Contrasena,
                Direccion = dto.Direccion,
                Ciudad = dto.Ciudad,
                Provincia = dto.Provincia,
                CodigoPostal = dto.CodigoPostal
            });

            return nuevoId > 0
                ? StatusCode(201, new { id = nuevoId })
                : BadRequest(new { error = "No se pudo registrar" });
        }
    }
}
