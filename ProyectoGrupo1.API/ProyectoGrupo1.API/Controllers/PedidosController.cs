using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.API.Models;
using ProyectoGrupo1.API.Services;
using System;

namespace ProyectoGrupo1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly PedidoService _pedidoService;

        public PedidosController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        // GET api/pedidos/{usuarioId}
        [HttpGet("{usuarioId}")]
        public IActionResult GetHistorialPedidos(int usuarioId)
        {
            try
            {
                var pedidos = _pedidoService.ObtenerHistorialPedidos(usuarioId);
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener pedidos: {ex.Message}");
            }
        }

        // GET api/pedidos/estados
        [HttpGet("estados")]
        public IActionResult GetEstadosPedido()
        {
            try
            {
                var estados = _pedidoService.ObtenerEstadosPedido();
                return Ok(estados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener estados: {ex.Message}");
            }
        }

        // GET api/pedidos/productos
        [HttpGet("productos")]
        public IActionResult GetProductosPTC()
        {
            try
            {
                var productos = _pedidoService.ObtenerProductosPTC();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener productos: {ex.Message}");
            }
        }

        // POST api/pedidos
        [HttpPost]
        public IActionResult CrearPedido([FromBody] NuevoPedidoInputModel nuevoPedido)
        {
            try
            {
                int usuarioId = nuevoPedido.UsuarioId;
                _pedidoService.CrearPedidoConDetalles(usuarioId, nuevoPedido);
                return Created("", new { mensaje = "Pedido creado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear pedido: {ex.Message}");
            }
        }
    }
}