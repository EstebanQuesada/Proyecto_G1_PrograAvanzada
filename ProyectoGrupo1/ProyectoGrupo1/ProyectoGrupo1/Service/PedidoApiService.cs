using System.Net;
using System.Net.Http.Json;
using ProyectoGrupo1.Models;

namespace ProyectoGrupo1.Service
{
    public class PedidoApiService
    {
        private readonly HttpClient _http;
        public PedidoApiService(HttpClient http) => _http = http;

        public record CrearDetalle(int PTCID, int Cantidad, decimal PrecioUnitario);
        public record CrearPedido(int UsuarioID, List<CrearDetalle> Detalles);

        public async Task<(bool ok, int pedidoId, string? error)> CrearAsync(
            int usuarioId, IEnumerable<DetalleCarrito> detalles, CancellationToken ct = default)
        {
            if (_http.BaseAddress is null)
                throw new InvalidOperationException("PedidoApiService no tiene BaseAddress configurado. Revisa Program.cs (AddHttpClient<PedidoApiService>).");

            var payload = new CrearPedido(
                usuarioId,
                detalles.Select(d => new CrearDetalle(d.PTCID, d.Cantidad, d.PrecioUnitario)).ToList()
            );

            var resp = await _http.PostAsJsonAsync("api/v1/pedidos", payload, ct);

            if (resp.IsSuccessStatusCode)
            {
                var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, int>>(cancellationToken: ct);
                return (true, json?["pedidoId"] ?? 0, null);
            }

            var body = await resp.Content.ReadAsStringAsync(ct);

            if (resp.StatusCode == HttpStatusCode.Conflict) 
                return (false, 0, "Stock insuficiente para uno o más productos.");

            return (false, 0, string.IsNullOrWhiteSpace(body) ? "No se pudo crear el pedido." : body);
        }

        public record HistItem(
            int PedidoID, DateTime FechaPedido, string NombreEstado,
            int DetallePedidoID, int PTCID, string Producto, int Stock,
            string NombreColor, string NombreTalla, int Cantidad,
            decimal PrecioUnitario, string? UrlImagen
        );

        public async Task<HistorialPedidoViewModel> HistorialAsync(int usuarioId, CancellationToken ct = default)
        {
            if (_http.BaseAddress is null)
                throw new InvalidOperationException("PedidoApiService no tiene BaseAddress configurado. Revisa Program.cs (AddHttpClient<PedidoApiService>).");

            var resp = await _http.GetAsync($"api/v1/pedidos/historial/{usuarioId}", ct);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync(ct);
                throw new ApplicationException(string.IsNullOrWhiteSpace(body)
                    ? "No se pudo obtener el historial."
                    : body);
            }

            var items = await resp.Content.ReadFromJsonAsync<List<HistItem>>(cancellationToken: ct) ?? new();

            var agrupado = items
                .GroupBy(i => new { i.PedidoID, i.FechaPedido, i.NombreEstado })
                .Select(g => new PedidoHistorialVm
                {
                    PedidoID = g.Key.PedidoID,
                    FechaPedido = g.Key.FechaPedido,
                    NombreEstado = g.Key.NombreEstado,
                    Detalles = g.Select(d => new PedidoHistorialDetalleVm
                    {
                        DetallePedidoID = d.DetallePedidoID,
                        PTCID = d.PTCID,
                        Producto = d.Producto,
                        NombreTalla = d.NombreTalla,
                        NombreColor = d.NombreColor,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        UrlImagen = d.UrlImagen
                    }).ToList()
                })
                .OrderByDescending(p => p.FechaPedido)
                .ToList();

            return new HistorialPedidoViewModel { Pedidos = agrupado };
        }
    }
}
