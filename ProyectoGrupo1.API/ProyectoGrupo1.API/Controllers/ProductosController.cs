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
            if (precioMin is > 0 && precioMax is > 0 && precioMin > precioMax)
                return Problem("El precio mínimo no puede ser mayor que el máximo.", statusCode: 400);

            var data = await _repo.CatalogoAsync(busqueda?.Trim(), categoria?.Trim(), precioMin, precioMax);
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDetalleDto>> Detalle(int id)
        {
            var dto = await _repo.DetalleAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpGet("categorias")]
        [ResponseCache(Duration = 120)] 
        public async Task<ActionResult<IEnumerable<string>>> Categorias()
        {
            var cats = await _repo.CategoriasAsync();
            return Ok(cats);
        }

        [HttpGet("ptc/{ptcId:int}")]
        public async Task<ActionResult<ProductoTallaColorDto>> GetPtc(int ptcId)
        {
            var ptc = await _repo.PtcPorIdAsync(ptcId);
            return ptc is null ? NotFound() : Ok(ptc);
        }
    }
    
}

