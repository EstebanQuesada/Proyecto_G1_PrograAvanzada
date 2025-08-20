using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProyectoGrupo1.Models;
using ProyectoGrupo1.Service;
using System.Linq;
using System.Threading.Tasks;
using MCarrito = ProyectoGrupo1.Models.Carrito;

namespace ProyectoGrupo1.Controllers
{
    public class CarritoController : Controller
    {
        private readonly CarritoService _carritoService;
        private readonly ApiProductoClient _api;
        private readonly ILogger<CarritoController> _logger;

        public CarritoController(
     CarritoService carritoService,
     ApiProductoClient api,
     ILogger<CarritoController> logger)
        {
            _carritoService = carritoService ?? throw new ArgumentNullException(nameof(carritoService));
            _api = api ?? throw new ArgumentNullException(nameof(api));   
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        private int? GetUsuarioId() => HttpContext.Session.GetInt32("UsuarioID");

        public IActionResult Index()
        {
            var usuarioId = GetUsuarioId();
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");

            var vm = _carritoService.ObtenerCarritoPorUsuario(usuarioId.Value);

            var model = new Carrito
            {
                UsuarioID = usuarioId.Value,
                Detalles = vm.Detalles.Select(d => new DetalleCarrito
                {
                    DetalleID = d.DetalleID,
                    PTCID = d.PTCID,
                    Cantidad = d.Cantidad,
                    NombreProducto = d.NombreProducto ?? "",
                    UrlImagen = d.UrlImagen ?? "",
                    NombreTalla = d.NombreTalla ?? "",
                    NombreColor = d.NombreColor ?? "",
                    PrecioUnitario = d.PrecioUnitario
                }).ToList()
            };

            return View(model); 
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(int ptcId, int cantidad)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");
            if (cantidad < 1) cantidad = 1;

            var ptc = await _api.PtcAsync(ptcId);
            if (ptc == null)
            {
                TempData["Error"] = "La combinación no existe.";
                return RedirectToAction("Index", "Carrito");
            }

            var carrito = _carritoService.ObtenerCarritoPorUsuario(usuarioId.Value);
            var existente = carrito.Detalles.FirstOrDefault(d => d.PTCID == ptcId);
            var yaEnCarrito = existente?.Cantidad ?? 0;
            var deseadoTotal = yaEnCarrito + cantidad;
            var totalCapado = Math.Min(deseadoTotal, ptc.Stock);
            if (totalCapado == yaEnCarrito)
            {
                TempData["Error"] = "No hay stock disponible para agregar más unidades.";
                return RedirectToAction("Index", "Carrito");
            }

            var prod = await _api.DetalleAsync(ptc.ProductoID);
            var precio = prod?.Precio ?? 0m;
            var imagen = prod?.Imagenes?.FirstOrDefault() ?? string.Empty;
            var nombre = prod?.Nombre ?? string.Empty;

            _carritoService.AgregarOActualizarProducto(
                usuarioId.Value,
                ptcId,
                totalCapado,
                maxStock: ptc.Stock,
                nombreProducto: nombre,
                nombreTalla: ptc.NombreTalla,
                nombreColor: ptc.NombreColor,
                precioUnitario: precio,
                urlImagen: imagen
            );

            return RedirectToAction("Index", "Carrito");
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarCantidad(int detalleId, int cantidad)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");
            if (cantidad < 1) cantidad = 1;

            var carrito = _carritoService.ObtenerCarritoPorUsuario(usuarioId.Value);
            var det = carrito.Detalles.FirstOrDefault(d => d.DetalleID == detalleId);
            if (det == null) return RedirectToAction("Index");

            var ptc = await _api.PtcAsync(det.PTCID);
            if (ptc == null) { TempData["Error"] = "No se pudo validar el stock."; return RedirectToAction("Index"); }

            var nuevaCant = Math.Min(cantidad, ptc.Stock);

            var prod = await _api.DetalleAsync(ptc.ProductoID);
            _carritoService.AgregarOActualizarProducto(
                usuarioId.Value,
                det.PTCID,
                nuevaCant,
                maxStock: ptc.Stock,
                nombreProducto: prod?.Nombre,
                nombreTalla: ptc.NombreTalla,
                nombreColor: ptc.NombreColor,
                precioUnitario: prod?.Precio,
                urlImagen: prod?.Imagenes?.FirstOrDefault()
            );

            if (cantidad > ptc.Stock) TempData["Error"] = $"Cantidad ajustada al stock disponible ({ptc.Stock}).";
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        public IActionResult Vaciar()
        {
            var usuarioId = GetUsuarioId();
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");

            _carritoService.VaciarCarrito(usuarioId.Value);
            return RedirectToAction("Index");
        }
    }
}
