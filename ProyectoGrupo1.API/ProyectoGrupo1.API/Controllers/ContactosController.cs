using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.API.DTOs.Contacto;
using ProyectoGrupo1.API.Services;

namespace ProyectoGrupo1.API.Controllers
{
    [ApiController]
    [Route("api/v1/contactos")]
    public class ContactosController : ControllerBase
    {
        private readonly IContactoService _service;
        private readonly ILogger<ContactosController> _logger;

        public ContactosController(IContactoService service, ILogger<ContactosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ContactoCrearDto dto)
        {
            if (dto == null)
                return BadRequest(new { error = "Datos de contacto inválidos." });

            try
            {
                var filas = await _service.CrearMensajeAsync(dto);
                return Created(string.Empty, new { filasAfectadas = filas });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando mensaje de contacto");
                return Problem("No se pudo crear el mensaje de contacto.", statusCode: 500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var mensajes = await _service.ListarMensajesAsync();
                return Ok(mensajes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo mensajes de contacto");
                return Problem("No se pudo obtener los mensajes.", statusCode: 500);
            }
        }
    }
}


