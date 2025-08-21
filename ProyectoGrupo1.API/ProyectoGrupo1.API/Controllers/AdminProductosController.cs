using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.API.DTOs.ProductAdmin;
using ProyectoGrupo1.API.Repositories;

namespace ProyectoGrupo1.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/productos")]
    public class AdminProductosController : ControllerBase
    {
        private readonly IAdminProductoRepository _repo;
        private readonly ILogger<AdminProductosController> _logger;

        public AdminProductosController(
            IAdminProductoRepository repo,
            ILogger<AdminProductosController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<object>> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? q = null)
        {
            var (total, items) = await _repo.ListarAsync(page, pageSize, q);
            return Ok(new { total, items });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<object>> Obtener(int id)
        {
            var (p, lookups) = await _repo.ObtenerAsync(id);
            if (id != 0 && p is null) return NotFound();
            return Ok(new { producto = p, lookups });
        }


        [HttpGet("lookups")]
        public async Task<ActionResult<AdminLookupsDto>> Lookups()
        {
            var (_, lookups) = await _repo.ObtenerAsync(0);
            return Ok(lookups);
        }



        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] AdminProductoSaveDto dto)
        {
            try
            {
                var id = await _repo.CrearAsync(dto);
                return Created(string.Empty, new { productoId = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando producto");
                return Problem("No se pudo crear el producto.", statusCode: 500);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Actualizar(int id, [FromBody] AdminProductoSaveDto dto)
        {
            try
            {
                var ok = await _repo.ActualizarAsync(id, dto);
                return ok ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando producto {Id}", id);
                return Problem("No se pudo actualizar el producto.", statusCode: 500);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                var ok = await _repo.EliminarAsync(id);
                return ok ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando producto {Id}", id);
                return Problem("No se pudo eliminar el producto.", statusCode: 500);
            }
        }
        [HttpPost("{id:int}/activar")]
        public async Task<ActionResult> Activar(int id)
        {
            try
            {
                var ok = await _repo.ActivarAsync(id); 
                return ok ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activando producto {Id}", id);
                return Problem("No se pudo activar el producto.", statusCode: 500);
            }
        }

    }
}
