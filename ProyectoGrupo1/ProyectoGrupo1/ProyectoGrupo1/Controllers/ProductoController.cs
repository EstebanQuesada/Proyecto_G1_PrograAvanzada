using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Models;
using ProyectoGrupo1.Service;

public class ProductoController : Controller
{
    private readonly ProductoService _productoService;

    public ProductoController(ProductoService productoService)
    {
        _productoService = productoService;
    }

    public IActionResult Catalogo(string categoria = null)
    {
        var productos = _productoService.ObtenerProductosCatalogo();

        if (!string.IsNullOrEmpty(categoria))
        {
            productos = productos.Where(p => p.Categoria == categoria).ToList();
        }

        ViewBag.Categorias = _productoService.ObtenerCategorias();
        ViewBag.CategoriaSeleccionada = categoria;

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
