using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Filters;
using ProyectoGrupo1.Services;   // ApiAdminUsuarioClient
using ProyectoGrupo1.Models;     // AdminCrearVm / AdminEditarVm
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace ProyectoGrupo1.Controllers
{
    [AdminOnly]
    public class AdminUsuariosController : Controller
    {
        private readonly ApiAdminUsuarioClient _api;
        public AdminUsuariosController(ApiAdminUsuarioClient api) => _api = api;

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? q = null)
        {
            try
            {
                var data = await _api.ListarAsync(page, pageSize, q)
                           ?? new ApiAdminUsuarioClient.PagedResult<ApiAdminUsuarioClient.AdminUserListItemDto>(
                                0, Enumerable.Empty<ApiAdminUsuarioClient.AdminUserListItemDto>());
                return View(data);
            }
            catch (HttpRequestException ex)
            {
                TempData["Msg"] = $"Error consultando API: {(int?)ex.StatusCode ?? 0} - {ex.Message}";
                return View(new ApiAdminUsuarioClient.PagedResult<ApiAdminUsuarioClient.AdminUserListItemDto>(
                    0, Enumerable.Empty<ApiAdminUsuarioClient.AdminUserListItemDto>()));
            }
        }

        [HttpGet]
        public IActionResult Crear() => View(new AdminCrearVm());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(AdminCrearVm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var ok = await _api.CrearAsync(new
            {
                vm.Nombre,
                vm.Apellido,
                vm.Correo,
                Contrasena = vm.Contrasena,
                vm.RolID,
                vm.Direccion,
                vm.Ciudad,
                vm.Provincia,
                vm.CodigoPostal
            });
            if (ok) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "No se pudo crear el usuario.");
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var u = await _api.ObtenerAsync(id);
            if (u == null) return NotFound();
            var vm = new AdminEditarVm
            {
                UsuarioID = u.UsuarioID,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Correo = u.Correo,
                RolID = u.RolID,
                Direccion = u.Direccion,
                Ciudad = u.Ciudad,
                Provincia = u.Provincia,
                CodigoPostal = u.CodigoPostal
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(AdminEditarVm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var ok = await _api.ActualizarAsync(vm.UsuarioID, new
            {
                vm.UsuarioID,
                vm.Nombre,
                vm.Apellido,
                vm.Correo,
                vm.RolID,
                vm.Direccion,
                vm.Ciudad,
                vm.Provincia,
                vm.CodigoPostal
            });
            if (ok) return RedirectToAction(nameof(Index));
            ModelState.AddModelError("", "No se pudo actualizar.");
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarRol(int id, int rolId)
        {
            await _api.CambiarRolAsync(id, rolId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Bloquear(int id, bool bloqueado)
        {
            await _api.BloquearAsync(id, bloqueado);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(int id, string nueva)
        {
            if (string.IsNullOrWhiteSpace(nueva))
            {
                TempData["Msg"] = "La contraseña no puede estar vacía.";
                return RedirectToAction(nameof(Index));
            }
            await _api.ResetPasswordAsync(id, nueva);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _api.EliminarAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Restaurar(int id)
        {
            await _api.RestaurarAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
