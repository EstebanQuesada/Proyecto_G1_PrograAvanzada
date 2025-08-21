
using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.Services;

namespace ProyectoGrupo1.Controllers
{

    public class AdminContactoController : Controller
    {
        private readonly ApiAdminContactoClient _api;
        private readonly ILogger<AdminContactoController> _logger;

        public AdminContactoController(ApiAdminContactoClient api, ILogger<AdminContactoController> logger)
        {
            _api = api;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? buscar, string? estado, int page = 1, int pageSize = 20, CancellationToken ct = default)
        {
            var data = await _api.ListarAsync(buscar, estado, page, pageSize, ct);
            ViewBag.Buscar = buscar;
            ViewBag.Estado = estado;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = data.Total;
            return View(data.Items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarVisto(int id, bool visto, bool completado, string? returnUrl = null, CancellationToken ct = default)
        {
            await _api.ActualizarEstadoAsync(id, true, completado, ct);
            return Redirect(returnUrl ?? Url.Action(nameof(Index))!);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarCompletado(int id, bool visto, bool completado, string? returnUrl = null, CancellationToken ct = default)
        {
            await _api.ActualizarEstadoAsync(id, visto, true, ct);
            return Redirect(returnUrl ?? Url.Action(nameof(Index))!);
        }
    }
}
