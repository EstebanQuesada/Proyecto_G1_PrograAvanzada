using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProyectoGrupo1.Models;

namespace ProyectoGrupo1.Services
{
    public class PedidoApiService
    {
        private readonly HttpClient _httpClient;

        public PedidoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7071/");
        }

        public async Task<List<Pedido>> ObtenerHistorialPedidosAsync(int usuarioId)
        {
            var response = await _httpClient.GetAsync($"api/pedidos/{usuarioId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Pedido>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Pedido>();
        }

        public async Task<List<EstadoPedido>> ObtenerEstadosPedidoAsync()
        {
            var response = await _httpClient.GetAsync("api/pedidos/estados");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EstadoPedido>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<EstadoPedido>();
        }

        public async Task<List<ProductoTallaColor>> ObtenerProductosPTCAsync()
        {
            var response = await _httpClient.GetAsync("api/pedidos/productos");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProductoTallaColor>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ProductoTallaColor>();
        }

        public async Task<bool> CrearPedidoConDetallesAsync(int usuarioId, NuevoPedidoInputModel nuevoPedido)
        {
            var json = JsonSerializer.Serialize(nuevoPedido);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/pedidos", content);
            return response.IsSuccessStatusCode;
        }
    }
}