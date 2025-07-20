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
                bool registrado = _usuarioService.RegistrarUsuario(usuario);
                if (registrado)
                    return RedirectToAction("Login");

                ViewBag.Mensaje = "No se pudo registrar el usuario.";
                return View(usuario);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(usuario);
            }
        }

        [HttpGet]
        public IActionResult Perfil()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null) return RedirectToAction("Login");

            var usuario = _usuarioService.ObtenerPerfilCompleto(usuarioId.Value);
            return View(usuario);
        }

        [HttpPost]
        public IActionResult ActualizarPerfil(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                var datosCompletos = _usuarioService.ObtenerPerfilCompleto(usuario.UsuarioID);
                return View("Perfil", datosCompletos);
            }

            var exito = _usuarioService.ActualizarPerfilYDireccion(usuario);
            TempData["Mensaje"] = exito ? "Perfil actualizado correctamente" : "No se pudo actualizar el perfil";

            if (exito)
                HttpContext.Session.SetString("Nombre", usuario.Nombre);

            var usuarioActualizado = _usuarioService.ObtenerPerfilCompleto(usuario.UsuarioID);
            return View("Perfil", usuarioActualizado);
        }

        [HttpGet]
        public IActionResult CambiarContrasena()
        {
            if (HttpContext.Session.GetInt32("UsuarioID") == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public IActionResult CambiarContrasena(string ContrasenaActual, string NuevaContrasena, string ConfirmarContrasena)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null) return RedirectToAction("Login");

            if (NuevaContrasena != ConfirmarContrasena)
            {
                TempData["MensajeClave"] = "La nueva contraseña y la confirmación no coinciden.";
                return View();
            }

            bool cambiado = _usuarioService.CambiarContrasena(usuarioId.Value, ContrasenaActual, NuevaContrasena);

            TempData["MensajeClave"] = cambiado
                ? "Contraseña cambiada exitosamente."
                : "La contraseña actual no es válida.";

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
