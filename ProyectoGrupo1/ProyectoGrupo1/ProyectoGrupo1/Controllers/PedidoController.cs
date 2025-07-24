using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Models;
using ProyectoGrupo1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ProyectoGrupo1.Controllers
{
    public class PedidoController : Controller
    {
        private readonly PedidoService _pedidoService;

        public PedidoController(IConfiguration config)
        {
            _pedidoService = new PedidoService(config);
        }

        // GET: /Pedido/Index
        [HttpGet]
        public IActionResult Index()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            // Obtener historial de pedidos
            var pedidos = _pedidoService.ObtenerHistorialPedidos(usuarioId.Value);

            // Obtener lista de estados
            var estados = _pedidoService.ObtenerEstadosPedido();

            // Obtener lista de productos
            var productosPTC = _pedidoService.ObtenerProductosPTC();

            var model = new HistorialPedidoViewModel
            {
                Pedidos = pedidos,
                Estados = estados,
                ProductosPTC = productosPTC,
                NuevoPedido = new NuevoPedidoInputModel()
            };

            return View(model);
        }

        // POST: /Pedido/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(HistorialPedidoViewModel model)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            if (!ModelState.IsValid)
            {
                model.Pedidos = _pedidoService.ObtenerHistorialPedidos(usuarioId.Value);
                model.Estados = _pedidoService.ObtenerEstadosPedido();
                model.ProductosPTC = _pedidoService.ObtenerProductosPTC();
                return View(model);
            }

            try
            {
                // Insertar el nuevo pedido y sus detalles
                _pedidoService.CrearPedidoConDetalles(usuarioId.Value, model.NuevoPedido);

                TempData["Mensaje"] = "Pedido agregado correctamente.";

                // Refrescar la página y mostrar el nuevo historial
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al agregar pedido: " + ex.Message);

                model.Pedidos = _pedidoService.ObtenerHistorialPedidos(usuarioId.Value);
                model.Estados = _pedidoService.ObtenerEstadosPedido();
                model.ProductosPTC = _pedidoService.ObtenerProductosPTC();
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            // Obtener datos de usuario y dirección
            var usuarioService = new ProyectoGrupo1.Services.UsuarioService(HttpContext.RequestServices.GetService<IConfiguration>());
            var usuario = usuarioService.ObtenerPerfilCompleto(usuarioId.Value);

            // Obtener carrito
            var carritoService = new ProyectoGrupo1.Service.CarritoService(HttpContext.RequestServices.GetService<IConfiguration>());
            var carrito = carritoService.ObtenerCarritoPorUsuario(usuarioId.Value);

            ViewBag.Carrito = carrito;
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Checkout(string Nombre, string Apellido, string Direccion, string Ciudad, string Provincia, string CodigoPostal, string MetodoPago)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            // Actualizar datos de usuario y dirección
            var usuarioService = new ProyectoGrupo1.Services.UsuarioService(HttpContext.RequestServices.GetService<IConfiguration>());
            var usuario = usuarioService.ObtenerPerfilCompleto(usuarioId.Value);
            usuario.Nombre = Nombre;
            usuario.Apellido = Apellido;
            usuario.Direccion = Direccion;
            usuario.Ciudad = Ciudad;
            usuario.Provincia = Provincia;
            usuario.CodigoPostal = CodigoPostal;
            usuarioService.ActualizarPerfilYDireccion(usuario);

            // Obtener carrito
            var carritoService = new ProyectoGrupo1.Service.CarritoService(HttpContext.RequestServices.GetService<IConfiguration>());
            var carrito = carritoService.ObtenerCarritoPorUsuario(usuarioId.Value);

            // Registrar pedido y detalles
            var pedidoService = new ProyectoGrupo1.Services.PedidoService(HttpContext.RequestServices.GetService<IConfiguration>());
            var nuevoPedido = new ProyectoGrupo1.Models.NuevoPedidoInputModel
            {
                EstadoID = 1, // Pendiente
                Detalles = carrito.Detalles.Select(d => new ProyectoGrupo1.Models.NuevoDetallePedidoInputModel
                {
                    PTCID = d.PTCID,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList()
            };
            pedidoService.CrearPedidoConDetalles(usuarioId.Value, nuevoPedido);

            // Limpiar carrito
            carritoService.VaciarCarrito(usuarioId.Value);

            // Mostrar confirmación
            TempData["Confirmacion"] = "¡Tu pedido ha sido registrado exitosamente!";
            return RedirectToAction("Confirmacion");
        }

        public IActionResult Confirmacion()
        {
            ViewBag.Mensaje = TempData["Confirmacion"];
            return View();
        }
    }
}