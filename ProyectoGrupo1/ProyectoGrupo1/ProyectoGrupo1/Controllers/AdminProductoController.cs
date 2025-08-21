using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Service;
using Microsoft.AspNetCore.Http;   

namespace ProyectoGrupo1.Controllers
{
    [Route("AdminProducto")]
    public class AdminProductoController : Controller
    {
        private readonly ApiAdminProductoClient _api;

        public AdminProductoController(ApiAdminProductoClient api) => _api = api;

        private bool NoEsAdmin() => HttpContext.Session.GetInt32("RolID") != 2;


        [HttpGet("")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? q = null)
        {
            var data = await _api.ListarAsync(page, pageSize, q);
            ViewBag.Page = page; ViewBag.PageSize = pageSize; ViewBag.Q = q;
            return View(data);
        }

        [HttpGet("crear")]
        public async Task<IActionResult> Crear()
        {
            if (NoEsAdmin()) return RedirectToAction("Index", "Home");

            var ob = await _api.ObtenerAsync(0);
            ViewBag.Lookups = ob?.lookups;

            return View(new ApiAdminProductoClient.AdminProductoSaveVm(
                "", "", 0, 0, 0, 0, new(), new()));
        }

        [HttpPost("crear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ApiAdminProductoClient.AdminProductoSaveVm vm)
        {
            if (NoEsAdmin()) return RedirectToAction("Index", "Home");

            var (ok, id, err) = await _api.CrearAsync(vm);
            if (!ok)
            {
                TempData["Error"] = err ?? "No se pudo crear.";
                TempData.Remove("Mensaje");
                return RedirectToAction(nameof(Crear));
            }

            TempData.Remove("Error");
            TempData["Mensaje"] = "Producto creado.";
            return RedirectToAction(nameof(Index));
        }




        [HttpGet("editar/{id:int}")]
        public async Task<IActionResult> Editar(int id)
        {
            var ob = await _api.ObtenerAsync(id);
            if (ob is null) return NotFound();

            ViewBag.Lookups = ob.lookups;
            ViewBag.ProductoID = ob.producto.ProductoID;

            var vm = new ApiAdminProductoClient.AdminProductoSaveVm(
                ob.producto.Nombre, ob.producto.Descripcion, ob.producto.Precio,
                ob.producto.CategoriaID, ob.producto.MarcaID, ob.producto.ProveedorID,
                ob.producto.Imagenes, ob.producto.PTCs
            );
            return View(vm);
        }

        [HttpPost("editar/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, ApiAdminProductoClient.AdminProductoSaveVm vm)
        {
            if (NoEsAdmin()) return RedirectToAction("Index", "Home");

            TempData.Remove("Error");
            var (ok, err) = await _api.ActualizarAsync(id, vm);

            if (!ok)
            {
                if (!string.IsNullOrWhiteSpace(err) && err.Contains("no existe", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Mensaje"] = err;         
                    TempData.Remove("Error");
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = string.IsNullOrWhiteSpace(err) ? "No se pudo actualizar." : err;
                TempData.Remove("Mensaje");
                return RedirectToAction(nameof(Editar), new { id });
            }

            TempData["Mensaje"] = "Producto actualizado.";
            TempData.Remove("Error");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("activar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activar(int id)
        {
            if (NoEsAdmin()) return RedirectToAction("Index", "Home");

            var (ok, err) = await _api.ActivarAsync(id);
            if (!ok)
            {
                TempData["Error"] = err ?? "No se pudo activar.";
                TempData.Remove("Mensaje");
                return RedirectToAction(nameof(Index));
            }

            TempData.Remove("Error");
            TempData["Mensaje"] = "Producto activado.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (NoEsAdmin()) return RedirectToAction("Index", "Home");

            var (ok, err) = await _api.EliminarAsync(id);
            if (!ok)
            {
                TempData["Error"] = err ?? "No se pudo inactivar.";
                TempData.Remove("Mensaje");
                return RedirectToAction(nameof(Index));
            }

            TempData.Remove("Error");
            TempData["Mensaje"] = "Producto inactivado.";
            return RedirectToAction(nameof(Index));
        }
    }
}

