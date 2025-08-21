// Proyecto: ProyectoGrupo1.API
// Archivo: Controllers/ContactosController.cs
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Crear([FromBody] ContactoCrearDto dto)
        {
            var filas = await _service.CrearMensajeAsync(dto);

            return Created("api/v1/contactos", new { filasAfectadas = filas });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Listar()
        {
            var mensajes = await _service.ListarMensajesAsync();
            return Ok(mensajes);
        }
    }
}
