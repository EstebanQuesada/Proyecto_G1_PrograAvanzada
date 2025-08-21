using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.API.DTOs;
using ProyectoGrupo1.API.Repositories;

namespace ProyectoGrupo1.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/catalogos")]
    public class AdminCatalogosController : ControllerBase
    {
        private readonly ICatalogosRepository _repo;
        private readonly ILogger<AdminCatalogosController> _log;

        public AdminCatalogosController(ICatalogosRepository repo, ILogger<AdminCatalogosController> log)
        {
            _repo = repo; _log = log;
        }

        [HttpGet("categorias")] public async Task<IActionResult> Categorias() => Ok(await _repo.ListarCategoriasAsync());
        [HttpPost("categorias")]
        public async Task<IActionResult> CrearCategoria([FromBody] LookupItemDto dto)
            => Created(string.Empty, new { id = await _repo.CrearCategoriaAsync(dto.Nombre) });
        [HttpPut("categorias/{id:int}")]
        public async Task<IActionResult> ActualizarCategoria(int id, [FromBody] LookupItemDto dto)
            => (await _repo.ActualizarCategoriaAsync(id, dto.Nombre)) ? NoContent() : NotFound();
        [HttpDelete("categorias/{id:int}")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            try { return (await _repo.EliminarCategoriaAsync(id)) ? NoContent() : NotFound(); }
            catch (Exception ex) { _log.LogError(ex, "Eliminar categoria {Id}", id); return Problem("No se puede eliminar. Está en uso.", statusCode: 409); }
        }

        [HttpGet("marcas")] public async Task<IActionResult> Marcas() => Ok(await _repo.ListarMarcasAsync());
        [HttpPost("marcas")]
        public async Task<IActionResult> CrearMarca([FromBody] LookupItemDto dto)
            => Created(string.Empty, new { id = await _repo.CrearMarcaAsync(dto.Nombre) });
        [HttpPut("marcas/{id:int}")]
        public async Task<IActionResult> ActualizarMarca(int id, [FromBody] LookupItemDto dto)
            => (await _repo.ActualizarMarcaAsync(id, dto.Nombre)) ? NoContent() : NotFound();
        [HttpDelete("marcas/{id:int}")]
        public async Task<IActionResult> EliminarMarca(int id)
        {
            try { return (await _repo.EliminarMarcaAsync(id)) ? NoContent() : NotFound(); }
            catch (Exception ex) { _log.LogError(ex, "Eliminar marca {Id}", id); return Problem("No se puede eliminar. Está en uso.", statusCode: 409); }
        }

        [HttpGet("tallas")] public async Task<IActionResult> Tallas() => Ok(await _repo.ListarTallasAsync());
        [HttpPost("tallas")]
        public async Task<IActionResult> CrearTalla([FromBody] LookupItemDto dto)
            => Created(string.Empty, new { id = await _repo.CrearTallaAsync(dto.Nombre) });
        [HttpPut("tallas/{id:int}")]
        public async Task<IActionResult> ActualizarTalla(int id, [FromBody] LookupItemDto dto)
            => (await _repo.ActualizarTallaAsync(id, dto.Nombre)) ? NoContent() : NotFound();
        [HttpDelete("tallas/{id:int}")]
        public async Task<IActionResult> EliminarTalla(int id)
        {
            try { return (await _repo.EliminarTallaAsync(id)) ? NoContent() : NotFound(); }
            catch (Exception ex) { _log.LogError(ex, "Eliminar talla {Id}", id); return Problem("No se puede eliminar. Está en uso.", statusCode: 409); }
        }

        [HttpGet("colores")] public async Task<IActionResult> Colores() => Ok(await _repo.ListarColoresAsync());
        [HttpPost("colores")]
        public async Task<IActionResult> CrearColor([FromBody] LookupItemDto dto)
            => Created(string.Empty, new { id = await _repo.CrearColorAsync(dto.Nombre) });
        [HttpPut("colores/{id:int}")]
        public async Task<IActionResult> ActualizarColor(int id, [FromBody] LookupItemDto dto)
            => (await _repo.ActualizarColorAsync(id, dto.Nombre)) ? NoContent() : NotFound();
        [HttpDelete("colores/{id:int}")]
        public async Task<IActionResult> EliminarColor(int id)
        {
            try { return (await _repo.EliminarColorAsync(id)) ? NoContent() : NotFound(); }
            catch (Exception ex) { _log.LogError(ex, "Eliminar color {Id}", id); return Problem("No se puede eliminar. Está en uso.", statusCode: 409); }
        }

        [HttpGet("proveedores")] public async Task<IActionResult> Proveedores() => Ok(await _repo.ListarProveedoresAsync());
        [HttpPost("proveedores")]
        public async Task<IActionResult> CrearProveedor([FromBody] ProveedorSaveDto dto)
            => Created(string.Empty, new { id = await _repo.CrearProveedorAsync(dto) });
        [HttpPut("proveedores/{id:int}")]
        public async Task<IActionResult> ActualizarProveedor(int id, [FromBody] ProveedorSaveDto dto)
            => (await _repo.ActualizarProveedorAsync(id, dto)) ? NoContent() : NotFound();
        [HttpDelete("proveedores/{id:int}")]
        public async Task<IActionResult> EliminarProveedor(int id)
        {
            try { return (await _repo.EliminarProveedorAsync(id)) ? NoContent() : NotFound(); }
            catch (Exception ex) { _log.LogError(ex, "Eliminar proveedor {Id}", id); return Problem("No se puede eliminar. Está en uso.", statusCode: 409); }
        }
    }
}
