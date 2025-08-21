using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using ProyectoGrupo1.Models;
using ProyectoGrupo1.Services;

namespace ProyectoGrupo1.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class UsuarioController : Controller
    {
        private readonly ApiUsuarioClient _api;

        public UsuarioController(ApiUsuarioClient api) => _api = api;


        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Usuario u)
        {
            try
            {
                var perfil = await _api.LoginAsync(u.Correo, u.Contrasena);
                if (perfil == null)
                {
                    ViewBag.Mensaje = "Credenciales incorrectas.";
                    return View(u);
                }

                HttpContext.Session.SetInt32("UsuarioID", perfil.UsuarioID);
                HttpContext.Session.SetString("Correo", perfil.Correo);
                HttpContext.Session.SetString("Nombre", perfil.Nombre);
                HttpContext.Session.SetInt32("RolID", perfil.RolID);

                return RedirectToAction("Index", "Home");
            }
            catch (ApplicationException ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(u);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Mensaje = "No se pudo contactar la API: " + ex.Message;
                return View(u);
            }
        }


        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Usuario u)
        {
            if (!ModelState.IsValid) return View(u);

            var ok = await _api.RegisterAsync(new
            {
                u.Nombre,
                u.Apellido,
                u.Correo,
                Contrasena = u.Contrasena,
                u.Direccion,
                u.Ciudad,
                u.Provincia,
                u.CodigoPostal
            });

            if (ok) return RedirectToAction(nameof(Login));

            ViewBag.Mensaje = "No se pudo registrar el usuario.";
            return View(u);
        }


        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            var id = HttpContext.Session.GetInt32("UsuarioID");
            if (id is null) return RedirectToAction(nameof(Login));

            var p = await _api.ObtenerPerfilAsync(id.Value);
            if (p == null) return RedirectToAction(nameof(Login));

            var model = new Usuario
            {
                UsuarioID = p.UsuarioID,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                Correo = p.Correo,
                Direccion = p.Direccion,
                Ciudad = p.Ciudad,
                Provincia = p.Provincia,
                CodigoPostal = p.CodigoPostal,
                RolID = p.RolID
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPerfil(Usuario u)
        {
            var id = HttpContext.Session.GetInt32("UsuarioID");
            if (id is null) return RedirectToAction(nameof(Login));

            if (!ModelState.IsValid)
            {
                var p = await _api.ObtenerPerfilAsync(u.UsuarioID);
                if (p == null) return RedirectToAction(nameof(Login));

                var model = new Usuario
                {
                    UsuarioID = p.UsuarioID,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Correo = p.Correo,
                    Direccion = p.Direccion,
                    Ciudad = p.Ciudad,
                    Provincia = p.Provincia,
                    CodigoPostal = p.CodigoPostal,
                    RolID = p.RolID
                };
                return View("Perfil", model);
            }

            var ok = await _api.ActualizarPerfilAsync(new
            {
                u.UsuarioID,
                u.Nombre,
                u.Apellido,
                u.Correo,
                u.Direccion,
                u.Ciudad,
                u.Provincia,
                u.CodigoPostal
            });

            TempData["Mensaje"] = ok ? "Perfil actualizado correctamente" : "No se pudo actualizar el perfil";

            var p2 = await _api.ObtenerPerfilAsync(u.UsuarioID);
            if (ok) HttpContext.Session.SetString("Nombre", u.Nombre);

            var model2 = p2 == null ? u : new Usuario
            {
                UsuarioID = p2.UsuarioID,
                Nombre = p2.Nombre,
                Apellido = p2.Apellido,
                Correo = p2.Correo,
                Direccion = p2.Direccion,
                Ciudad = p2.Ciudad,
                Provincia = p2.Provincia,
                CodigoPostal = p2.CodigoPostal,
                RolID = p2.RolID
            };

            return View("Perfil", model2);
        }


        [HttpGet]
        public IActionResult CambiarContrasena()
            => HttpContext.Session.GetInt32("UsuarioID") == null ? RedirectToAction(nameof(Login)) : View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarContrasena(string ContrasenaActual, string NuevaContrasena, string ConfirmarContrasena)
        {
            var id = HttpContext.Session.GetInt32("UsuarioID");
            if (id is null) return RedirectToAction(nameof(Login));

            if (!string.Equals(NuevaContrasena, ConfirmarContrasena, StringComparison.Ordinal))
            {
                return View();
            }

            var (ok, _) = await _api.CambiarPasswordAsync(id.Value, ContrasenaActual, NuevaContrasena);

            if (ok)
            {
                TempData["MensajeClave"] = "Contraseña cambiada exitosamente.";
                return RedirectToAction(nameof(CambiarContrasena));
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult LogoutGet() => RedirectToAction("Index", "Home");
    }
}
