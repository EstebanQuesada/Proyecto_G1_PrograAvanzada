using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Models;
using ProyectoGrupo1.Service;
using Microsoft.AspNetCore.Http;

namespace ProyectoGrupo1.Controllers
{
    public class CarritoController : Controller
    {
        private readonly CarritoService _carritoService;
        private readonly ProductoService _productoService;

        public CarritoController(CarritoService carritoService, ProductoService productoService)
        {
            _carritoService = carritoService;
            _productoService = productoService;
        }

        private int? GetUsuarioId()
        {
            return HttpContext.Session.GetInt32("UsuarioID");
        }

        public IActionResult Index()
        {
            var usuarioId = GetUsuarioId();
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");
            var carrito = _carritoService.ObtenerCarritoPorUsuario(usuarioId.Value);
            return View(carrito);
        }

        [HttpPost]
        public IActionResult Agregar(int ptcId, int cantidad)
        {
            var usuarioId = GetUsuarioId();
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");
            _carritoService.AgregarOActualizarProducto(usuarioId.Value, ptcId, cantidad);
            TempData["Mensaje"] = "¡Producto agregado al carrito!";
            // Obtener el productoId a partir del ptcId para redirigir al detalle
            // (esto requiere una consulta, pero para simplificar, puedes pasar el productoId como campo oculto en el form)
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult ModificarCantidad(int detalleId, int cantidad)
        {
            var usuarioId = GetUsuarioId();
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");
            var carrito = _carritoService.ObtenerCarritoPorUsuario(usuarioId.Value);
            var detalle = carrito.Detalles.FirstOrDefault(d => d.DetalleID == detalleId);
            if (detalle != null)
            {
                _carritoService.AgregarOActualizarProducto(usuarioId.Value, detalle.PTCID, cantidad);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Eliminar(int detalleId)
        {
            var usuarioId = GetUsuarioId();
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");
            var carrito = _carritoService.ObtenerCarritoPorUsuario(usuarioId.Value);
            var detalle = carrito.Detalles.FirstOrDefault(d => d.DetalleID == detalleId);
            if (detalle != null)
            {
                _carritoService.EliminarProducto(usuarioId.Value, detalle.PTCID);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Vaciar()
        {
            var usuarioId = GetUsuarioId();
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");
            _carritoService.VaciarCarrito(usuarioId.Value);
            return RedirectToAction("Index");
        }
    }
}