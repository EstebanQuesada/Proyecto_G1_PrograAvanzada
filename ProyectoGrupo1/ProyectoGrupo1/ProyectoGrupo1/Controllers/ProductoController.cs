using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Models;
using ProyectoGrupo1.Service;
using System.Linq;
using Microsoft.Extensions.Logging;


public class ProductoController : Controller
{
    private readonly ApiProductoClient _api;
    private readonly ILogger<ProductoController> _logger;

    public ProductoController(ApiProductoClient api, ILogger<ProductoController> logger)
    {
        _api = api;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Catalogo(string? busqueda = null, string? categoria = null,
                                          decimal? precioMin = null, decimal? precioMax = null,
                                          CancellationToken ct = default)
    {
        try
        {
            var productos = await _api.CatalogoAsync(busqueda, categoria, precioMin, precioMax, ct);
            ViewBag.Categorias = await _api.CategoriasAsync(ct);
            ViewBag.CategoriaSeleccionada = categoria;
            ViewBag.Busqueda = busqueda;
            ViewBag.PrecioMin = precioMin;
            ViewBag.PrecioMax = precioMax;
            return View(productos);
        }
        catch (ApiClientException ex)
        {
            _logger.LogWarning(ex, "Error funcional en Catálogo. Status: {Status}. Body: {Body}",
                (int)ex.StatusCode, ex.RawBody);
            TempData["Error"] = "No se pudo cargar el catálogo. Intenta más tarde.";
            return View("Error");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado en Catálogo");
            TempData["Error"] = "Ocurrió un error al cargar el catálogo.";
            return View("Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Detalle(int id, CancellationToken ct = default)
    {
        try
        {
            var producto = await _api.DetalleAsync(id, ct);
            if (producto == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            ViewBag.PTCs = producto.PTCs ?? new List<ProductoTallaColor>();
            return View(producto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en Detalle {Id}", id);
            TempData["Error"] = "Ocurrió un error al cargar el detalle.";
            return View("Error");
        }
    }
}