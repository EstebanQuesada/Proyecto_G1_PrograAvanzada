using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.API.DTOs.Pedido;
using ProyectoGrupo1.API.Repositories;

namespace ProyectoGrupo1.API.Controllers
{
    [ApiController]
    [Route("api/v1/pedidos")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoRepository _repo;
        private readonly ILogger<PedidosController> _logger;

        public PedidosController(IPedidoRepository repo, ILogger<PedidosController> logger)
        {
            _repo = repo; _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] PedidoCrearDto dto)
        {
            if (dto is null || dto.Detalles?.Count == 0)
                return BadRequest(new { error = "No hay productos en el pedido." });

            try
            {
                var id = await _repo.CrearAsync(dto.UsuarioID, dto.Detalles);
                return Created(string.Empty, new { pedidoId = id });
            }
            catch (InvalidOperationException ex) when (ex.Message == "STOCK_INSUFICIENTE")
            {
                return Conflict(new { error = "Stock insuficiente para uno o más productos." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando pedido");
                return Problem("No se pudo crear el pedido.", statusCode: 500);
            }
        }

        [HttpGet("historial/{usuarioId:int}")]
        public async Task<ActionResult<IEnumerable<HistorialPedidoDto>>> Historial(int usuarioId)
        {
            try
            {
                var items = await _repo.HistorialAsync(usuarioId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo historial de pedidos {UsuarioID}", usuarioId);
                return Problem("No se pudo obtener el historial.", statusCode: 500);
            }
        }
    }
}
