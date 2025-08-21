
using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.API.DTOs.Contacto;
using ProyectoGrupo1.API.Services;

namespace ProyectoGrupo1.API.Controllers
{
    [ApiController]
    [Route("api/v1/contactos/admin")]
    public class ContactosAdminController : ControllerBase
    {
        private readonly IContactoService _service;
        private readonly ILogger<ContactosAdminController> _logger;

        public ContactosAdminController(IContactoService service, ILogger<ContactosAdminController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] string? buscar, [FromQuery] string? estado, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var (items, total) = await _service.ListarAdminAsync(buscar, estado, page, pageSize);
            return Ok(new { total, items });
        }

        [HttpPut("{id:int}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ContactoActualizarEstadoDto dto)
        {
            var filas = await _service.ActualizarEstadoAsync(id, dto.Visto, dto.Completado);
            if (filas == 0) return NotFound(new { error = "Mensaje no encontrado." });
            return NoContent();
        }
    }
}
