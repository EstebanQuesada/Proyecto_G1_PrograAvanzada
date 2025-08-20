using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProyectoGrupo1.API.DTOs.Product;
using ProyectoGrupo1.API.Repositories;

namespace ProyectoGrupo1.API.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepository _repo;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductoRepository repo, ILogger<ProductosController> logger)
        {
            _repo = repo; _logger = logger;
        }

        [HttpGet("catalogo")]
        public async Task<ActionResult<IEnumerable<ProductoCatalogoDto>>> Catalogo(
            [FromQuery] string? busqueda, [FromQuery] string? categoria,
            [FromQuery] decimal? precioMin, [FromQuery] decimal? precioMax)
        {
            try
            {
                var data = await _repo.CatalogoAsync(busqueda, categoria, precioMin, precioMax);
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo catálogo");
                return Problem("Ocurrió un error al obtener el catálogo.", statusCode: 500);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDetalleDto>> Detalle(int id)
        {
            try
            {
                var dto = await _repo.DetalleAsync(id);
                if (dto is null) return NotFound();
                return Ok(dto);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo detalle {Id}", id);
                return Problem("Ocurrió un error al obtener el detalle.", statusCode: 500);
            }
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<string>>> Categorias()
        {
            try
            {
                var cats = await _repo.CategoriasAsync();
                return Ok(cats);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo categorías");
                return Problem("Ocurrió un error al obtener las categorías.", statusCode: 500);
            }
        }
    }
}
