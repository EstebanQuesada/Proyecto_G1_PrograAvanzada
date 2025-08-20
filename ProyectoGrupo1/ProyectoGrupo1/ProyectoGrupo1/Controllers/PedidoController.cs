using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using ProyectoGrupo1.Models;
using ProyectoGrupo1.Services;   
using ProyectoGrupo1.Service;   

namespace ProyectoGrupo1.Controllers
{
    public class PedidoController : Controller
    {
        private readonly PedidoApiService _pedidoApiService;
        private readonly ApiUsuarioClient _api;       
        private readonly CarritoService _carrito;    

        public PedidoController(
            PedidoApiService pedidoApiService,
            ApiUsuarioClient api,
            CarritoService carrito)
        {
            _pedidoApiService = pedidoApiService;
            _api = api;
            _carrito = carrito;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var pedidos = await _pedidoApiService.ObtenerHistorialPedidosAsync(usuarioId.Value);
            var estados = await _pedidoApiService.ObtenerEstadosPedidoAsync();
            var productosPTC = await _pedidoApiService.ObtenerProductosPTCAsync();

            var model = new HistorialPedidoViewModel
            {
                Pedidos = pedidos,
                Estados = estados,
                ProductosPTC = productosPTC,
                NuevoPedido = new NuevoPedidoInputModel()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(HistorialPedidoViewModel model)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            if (!ModelState.IsValid)
            {
                model.Pedidos = await _pedidoApiService.ObtenerHistorialPedidosAsync(usuarioId.Value);
                model.Estados = await _pedidoApiService.ObtenerEstadosPedidoAsync();
                model.ProductosPTC = await _pedidoApiService.ObtenerProductosPTCAsync();
                return View(model);
            }

            var success = await _pedidoApiService.CrearPedidoConDetallesAsync(usuarioId.Value, model.NuevoPedido);

            if (success)
            {
                TempData["Mensaje"] = "Pedido agregado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Error al agregar pedido vía API.");
            model.Pedidos = await _pedidoApiService.ObtenerHistorialPedidosAsync(usuarioId.Value);
            model.Estados = await _pedidoApiService.ObtenerEstadosPedidoAsync();
            model.ProductosPTC = await _pedidoApiService.ObtenerProductosPTCAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var carrito = _carrito.ObtenerCarritoPorUsuario(usuarioId.Value);
            carrito.Detalles ??= new List<CarritoDetalle>();
            ViewBag.Carrito = carrito;

            var perfil = await _api.ObtenerPerfilAsync(usuarioId.Value);
            if (perfil == null) return RedirectToAction("Login", "Usuario");

            var usuario = new Usuario
            {
                UsuarioID = perfil.UsuarioID,
                Nombre = perfil.Nombre,
                Apellido = perfil.Apellido,
                Correo = perfil.Correo,
                Direccion = perfil.Direccion,
                Ciudad = perfil.Ciudad,
                Provincia = perfil.Provincia,
                CodigoPostal = perfil.CodigoPostal,
                RolID = perfil.RolID
            };

            return View(usuario);
        }

        [HttpPost]
        public IActionResult Checkout(string Nombre, string Apellido, string Direccion,
                                      string Ciudad, string Provincia, string CodigoPostal,
                                      string MetodoPago)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            return RedirectToAction("Confirmacion");
        }

        public IActionResult Confirmacion()
        {
            ViewBag.Mensaje = TempData["Confirmacion"];
            return View();
        }

        [HttpPost]
        public IActionResult VolverAPedir(int pedidoId)
        {
            return RedirectToAction("Index");
        }
    }
}
