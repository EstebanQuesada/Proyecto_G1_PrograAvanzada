using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Service;

namespace ProyectoGrupo1.Controllers
{
    public class PedidoController : Controller
    {
        private readonly CarritoService _carrito;
        private readonly PedidoApiService _pedidos;
        private readonly ApiProductoClient _api; 
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(
            CarritoService carrito,
            PedidoApiService pedidos,
            ApiProductoClient api,
            ILogger<PedidoController> logger)
        {
            _carrito = carrito; _pedidos = pedidos; _api = api; _logger = logger;
        }

        private int? GetUsuarioId() => HttpContext.Session.GetInt32("UsuarioID");

        [HttpGet]
        public IActionResult Checkout()
        {
            var uid = GetUsuarioId();
            if (uid is null) return RedirectToAction("Login", "Usuario");

            var cart = _carrito.ObtenerCarritoPorUsuario(uid.Value);
            if (cart.Detalles.Count == 0)
            {
                TempData["Error"] = "Tu carrito está vacío.";
                return RedirectToAction("Index", "Carrito");
            }
            return View(cart);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmar()
        {
            var uid = GetUsuarioId();
            if (uid is null) return RedirectToAction("Login", "Usuario");

            var cart = _carrito.ObtenerCarritoPorUsuario(uid.Value);
            if (cart.Detalles.Count == 0)
            {
                TempData["Error"] = "Tu carrito está vacío.";
                return RedirectToAction("Index", "Carrito");
            }

            try
            {
                var (ok, pedidoId, error) = await _pedidos.CrearAsync(uid.Value, cart.Detalles);
                if (!ok)
                {
                    TempData["Error"] = error ?? "No se pudo crear el pedido.";
                    return RedirectToAction("Index", "Carrito");
                }

                _carrito.VaciarCarrito(uid.Value);

                TempData["Mensaje"] = $"¡Pedido #{pedidoId} creado con éxito!";
                return RedirectToAction("Index", "Carrito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirmando pedido para usuario {Uid}", uid);
                TempData["Error"] = "Ocurrió un error al confirmar el pedido.";
                return RedirectToAction("Index", "Carrito");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Historial()
        {
            var uid = GetUsuarioId();
            if (uid is null) return RedirectToAction("Login", "Usuario");

            try
            {
                var vm = await _pedidos.HistorialAsync(uid.Value);
                return View(vm); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo historial para usuario {Uid}", uid);
                TempData["Error"] = "No se pudo cargar tu historial.";
                return View(new ProyectoGrupo1.Models.HistorialPedidoViewModel());
            }
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> VolverAPedir(int pedidoId)
        {
            var uid = GetUsuarioId();
            if (uid is null) return RedirectToAction("Login", "Usuario");

            try
            {
                var vm = await _pedidos.HistorialAsync(uid.Value);
                var pedido = vm.Pedidos.FirstOrDefault(p => p.PedidoID == pedidoId);
                if (pedido is null || pedido.Detalles.Count == 0)
                {
                    TempData["Error"] = "No se encontró el pedido o no tiene productos.";
                    return RedirectToAction("Historial");
                }

                int agregados = 0, sinStock = 0;

                foreach (var d in pedido.Detalles)
                {
                    var ptc = await _api.PtcAsync(d.PTCID);
                    if (ptc is null || ptc.Stock <= 0) { sinStock++; continue; }

                    var prod = await _api.DetalleAsync(ptc.ProductoID);
                    var cantidad = Math.Min(d.Cantidad, ptc.Stock);

                    _carrito.AgregarOActualizarProducto(
                        uid.Value,
                        d.PTCID,
                        cantidad,
                        maxStock: ptc.Stock,
                        nombreProducto: prod?.Nombre ?? d.Producto,
                        nombreTalla: ptc.NombreTalla,
                        nombreColor: ptc.NombreColor,
                        precioUnitario: prod?.Precio ?? d.PrecioUnitario,
                        urlImagen: prod?.Imagenes?.FirstOrDefault() ?? d.UrlImagen
                    );

                    agregados += cantidad;
                }

                TempData["Mensaje"] = $"Se agregaron {agregados} artículo(s) al carrito."
                                        + (sinStock > 0 ? $" {sinStock} item(s) sin stock fueron omitidos." : "");
                return RedirectToAction("Index", "Carrito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al volver a pedir {PedidoId} para usuario {Uid}", pedidoId, uid);
                TempData["Error"] = "No se pudo reprocesar el pedido.";
                return RedirectToAction("Historial");
            }
        }
    }
}
