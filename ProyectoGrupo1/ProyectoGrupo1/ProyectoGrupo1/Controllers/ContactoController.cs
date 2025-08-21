
using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Models;
using ProyectoGrupo1.Services;

namespace ProyectoGrupo1.Controllers
{
    public class ContactoController : Controller
    {
        private readonly TextoService _textoService;
        private readonly IContactoApiClient _apiClient;
        private readonly ILogger<ContactoController> _logger;

        public ContactoController(TextoService textoService, IContactoApiClient apiClient, ILogger<ContactoController> logger)
        {
            _textoService = textoService;
            _apiClient = apiClient;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Descripcion = _textoService.ObtenerDescripcionContacto();
            ViewBag.Confirmacion = TempData["Confirmacion"];
            return View(new ContactoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactoViewModel modelo, CancellationToken ct)
        {
            ViewBag.Descripcion = _textoService.ObtenerDescripcionContacto();

            if (!ModelState.IsValid)
                return View(modelo);

            try
            {
                await _apiClient.CrearAsync(modelo, ct);
                TempData["Confirmacion"] = "¡Tu mensaje ha sido enviado correctamente!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando mensaje de contacto desde MVC");
                ModelState.AddModelError(string.Empty, "No se pudo enviar el mensaje. Intenta más tarde.");
                return View(modelo);
            }
        }
    }
}
