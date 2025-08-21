using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Service;

namespace ProyectoGrupo1.Controllers
{
    [Route("AdminCatalogos")]
    public class AdminCatalogosController : Controller
    {
        private readonly ApiCatalogosClient _api;
        public AdminCatalogosController(ApiCatalogosClient api) => _api = api;

        [HttpGet("")]
        public IActionResult Index() => RedirectToAction(nameof(Categorias));

        [HttpGet("Categorias")]
        public async Task<IActionResult> Categorias()
        {
            ViewBag.Titulo = "Categorías";
            ViewBag.Tipo = "categoria"; 
            var model = await _api.CategoriasAsync() ?? new List<ApiCatalogosClient.LookupVm>();
            return View("LookupCrud", model);
        }

        [HttpPost("CrearCategoria"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCategoria(string nombre)
        {
            var ok = !string.IsNullOrWhiteSpace(nombre) && await _api.CrearCategoriaAsync(nombre);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Categoría creada." : "No se pudo crear la categoría.";
            return RedirectToAction(nameof(Categorias));
        }

        [HttpPost("ActualizarCategoria"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarCategoria(int id, string nombre)
        {
            var ok = await _api.ActualizarCategoriaAsync(id, nombre);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Categoría actualizada." : "No se pudo actualizar la categoría.";
            return RedirectToAction(nameof(Categorias));
        }

        [HttpPost("EliminarCategoria"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var ok = await _api.EliminarCategoriaAsync(id);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Categoría eliminada." : "No se pudo eliminar la categoría.";
            return RedirectToAction(nameof(Categorias));
        }

        [HttpGet("Marcas")]
        public async Task<IActionResult> Marcas()
        {
            ViewBag.Titulo = "Marcas";
            ViewBag.Tipo = "marca";
            var model = await _api.MarcasAsync() ?? new List<ApiCatalogosClient.LookupVm>();
            return View("LookupCrud", model);
        }

        [HttpPost("CrearMarca"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearMarca(string nombre)
        {
            var ok = !string.IsNullOrWhiteSpace(nombre) && await _api.CrearMarcaAsync(nombre);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Marca creada." : "No se pudo crear la marca.";
            return RedirectToAction(nameof(Marcas));
        }

        [HttpPost("ActualizarMarca"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarMarca(int id, string nombre)
        {
            var ok = await _api.ActualizarMarcaAsync(id, nombre);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Marca actualizada." : "No se pudo actualizar la marca.";
            return RedirectToAction(nameof(Marcas));
        }

        [HttpPost("EliminarMarca"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarMarca(int id)
        {
            var ok = await _api.EliminarMarcaAsync(id);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Marca eliminada." : "No se pudo eliminar la marca.";
            return RedirectToAction(nameof(Marcas));
        }

        [HttpGet("Tallas")]
        public async Task<IActionResult> Tallas()
        {
            ViewBag.Titulo = "Tallas";
            ViewBag.Tipo = "talla";
            var model = await _api.TallasAsync() ?? new List<ApiCatalogosClient.LookupVm>();
            return View("LookupCrud", model);
        }

        [HttpPost("CrearTalla"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearTalla(string nombre)
        {
            var ok = !string.IsNullOrWhiteSpace(nombre) && await _api.CrearTallaAsync(nombre);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Talla creada." : "No se pudo crear la talla.";
            return RedirectToAction(nameof(Tallas));
        }

        [HttpPost("ActualizarTalla"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarTalla(int id, string nombre)
        {
            var ok = await _api.ActualizarTallaAsync(id, nombre);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Talla actualizada." : "No se pudo actualizar la talla.";
            return RedirectToAction(nameof(Tallas));
        }

        [HttpPost("EliminarTalla"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarTalla(int id)
        {
            var ok = await _api.EliminarTallaAsync(id);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Talla eliminada." : "No se pudo eliminar la talla.";
            return RedirectToAction(nameof(Tallas));
        }

        [HttpGet("Colores")]
        public async Task<IActionResult> Colores()
        {
            ViewBag.Titulo = "Colores";
            ViewBag.Tipo = "color";
            var model = await _api.ColoresAsync() ?? new List<ApiCatalogosClient.LookupVm>();
            return View("LookupCrud", model);
        }

        [HttpPost("CrearColor"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearColor(string nombre)
        {
            var ok = !string.IsNullOrWhiteSpace(nombre) && await _api.CrearColorAsync(nombre);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Color creado." : "No se pudo crear el color.";
            return RedirectToAction(nameof(Colores));
        }

        [HttpPost("ActualizarColor"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarColor(int id, string nombre)
        {
            var ok = await _api.ActualizarColorAsync(id, nombre);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Color actualizado." : "No se pudo actualizar el color.";
            return RedirectToAction(nameof(Colores));
        }

        [HttpPost("EliminarColor"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarColor(int id)
        {
            var ok = await _api.EliminarColorAsync(id);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Color eliminado." : "No se pudo eliminar el color.";
            return RedirectToAction(nameof(Colores));
        }

        [HttpGet("Proveedores")]
        public async Task<IActionResult> Proveedores()
        {
            var model = await _api.ProveedoresAsync() ?? new List<ApiCatalogosClient.ProveedorVm>();
            return View(model); 
        }

        [HttpPost("ProveedorCrear"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ProveedorCrear(string nombre, string correo, string telefono)
        {
            var ok = await _api.CrearProveedorAsync(new ApiCatalogosClient.ProveedorSaveVm(nombre, correo, telefono));
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Proveedor creado." : "No se pudo crear el proveedor.";
            return RedirectToAction(nameof(Proveedores));
        }

        [HttpPost("ProveedorGuardar"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ProveedorGuardar(int id, string nombre, string correo, string telefono)
        {
            var ok = await _api.ActualizarProveedorAsync(id, new ApiCatalogosClient.ProveedorSaveVm(nombre, correo, telefono));
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Proveedor actualizado." : "No se pudo actualizar el proveedor.";
            return RedirectToAction(nameof(Proveedores));
        }

        [HttpPost("ProveedorEliminar"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ProveedorEliminar(int id)
        {
            var ok = await _api.EliminarProveedorAsync(id);
            TempData[ok ? "Mensaje" : "Error"] = ok ? "Proveedor eliminado." : "No se pudo eliminar el proveedor.";
            return RedirectToAction(nameof(Proveedores));
        }
    }
}
