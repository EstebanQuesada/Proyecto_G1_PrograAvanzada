using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Models;
using ProyectoGrupo1.Services;
using Microsoft.AspNetCore.Http;

namespace ProyectoGrupo1.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(IConfiguration config)
        {
            _usuarioService = new UsuarioService(config);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(Usuario usuarioLogin)
        {
            var usuario = _usuarioService.ValidarUsuario(usuarioLogin);
            if (usuario != null)
            {
                HttpContext.Session.SetInt32("UsuarioID", usuario.UsuarioID);
                HttpContext.Session.SetString("Correo", usuario.Correo);
                HttpContext.Session.SetString("Nombre", usuario.Nombre);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Mensaje = "Credenciales incorrectas.";
            return View(usuarioLogin);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(Usuario usuario)
        {
            try
            {
                var exito = _usuarioService.RegistrarUsuario(usuario);
                if (exito)
                {
                    return RedirectToAction("Login");
                }

                ViewBag.Mensaje = "No se pudo registrar el usuario.";
                return View(usuario);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(usuario);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
