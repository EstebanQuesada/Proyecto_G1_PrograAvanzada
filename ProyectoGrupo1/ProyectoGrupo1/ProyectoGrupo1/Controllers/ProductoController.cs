using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Models;
using ProyectoGrupo1.Service;
using System.Linq;

namespace ProyectoGrupo1.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        public IActionResult Catalogo(
            string busqueda = null,
            string categoria = null,
            decimal? precioMin = null,
            decimal? precioMax = null)
        {
            var productos = _productoService.ObtenerProductosCatalogo();

            if (!string.IsNullOrEmpty(busqueda))
            {
                productos = productos
                    .Where(p => p.Nombre != null && p.Nombre.Contains(busqueda, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrEmpty(categoria))
            {
                productos = productos
                    .Where(p => p.Categoria == categoria)
                    .ToList();
            }

            if (precioMin.HasValue)
            {
                productos = productos
                    .Where(p => p.Precio >= precioMin.Value)
                    .ToList();
            }
            if (precioMax.HasValue)
            {
                productos = productos
                    .Where(p => p.Precio <= precioMax.Value)
                    .ToList();
            }

            ViewBag.Categorias = _productoService.ObtenerCategorias();
            ViewBag.CategoriaSeleccionada = categoria;
            ViewBag.Busqueda = busqueda;
            ViewBag.PrecioMin = precioMin;
            ViewBag.PrecioMax = precioMax;

            return View(productos);
        }

        public IActionResult Detalle(int id)
        {
            var producto = _productoService.ObtenerDetalleProducto(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }
    }
}

