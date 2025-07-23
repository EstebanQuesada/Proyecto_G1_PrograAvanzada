using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Models;
using ProyectoGrupo1.Services;

namespace ProyectoGrupo1.Controllers
{
    public class ContactoController : Controller
    {
        private readonly TextoService _textoService;
        private readonly ContactoService _contactoService;

        public ContactoController(TextoService textoService, ContactoService contactoService)
        {
            _textoService = textoService;
            _contactoService = contactoService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Descripcion = _textoService.ObtenerDescripcionContacto();
            return View(new ContactoViewModel());
        }

        [HttpPost]
        public IActionResult Index(ContactoViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                _contactoService.GuardarMensaje(modelo);
                ViewBag.Confirmacion = "¡Tu mensaje ha sido enviado correctamente!";
                ModelState.Clear();

                ViewBag.Descripcion = _textoService.ObtenerDescripcionContacto();
                return View(new ContactoViewModel());
            }

            ViewBag.Descripcion = _textoService.ObtenerDescripcionContacto();
            return View(modelo);
        }
    }
}




