
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoGrupo1.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
            try
            {
                var data = await _api.ListarAsync(buscar, estado, page, pageSize, ct);
                ViewBag.Buscar = buscar;
                ViewBag.Estado = estado;
                ViewBag.Page = page;
                ViewBag.PageSize = pageSize;
                ViewBag.Total = data.Total;
                return View(data.Items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando mensajes de contacto");
                TempData["Error"] = "No se pudieron cargar los mensajes.";
                ViewBag.Buscar = buscar;
                ViewBag.Estado = estado;
                ViewBag.Page = page;
                ViewBag.PageSize = pageSize;
                ViewBag.Total = 0;
                return View(new List<AdminContactoItemVm>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarVisto(int id, bool completado, string? returnUrl = null, CancellationToken ct = default)
        {
            await _api.ActualizarEstadoAsync(id, true, completado, ct);
            return Redirect(returnUrl ?? Url.Action(nameof(Index))!);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarCompletado(int id, string? returnUrl = null, CancellationToken ct = default)
        {
            await _api.ActualizarEstadoAsync(id, true, true, ct);
            return Redirect(returnUrl ?? Url.Action(nameof(Index))!);
        }
    }
}
