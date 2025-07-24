using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Models;
using ProyectoGrupo1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

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
    }
}