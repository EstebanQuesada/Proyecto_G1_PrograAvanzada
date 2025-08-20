/*
using Dapper;
using ProyectoGrupo1.Models;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ProyectoGrupo1.Services
{
    public class PedidoService
    {
        private readonly string _connectionString;

        public PedidoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TiendaRopaDB")
                ?? throw new Exception("No se encontró la cadena de conexión 'TiendaRopaDB'");
        }

        // Obtener historial de pedidos del usuario
        public List<Pedido> ObtenerHistorialPedidos(int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            var pedidoDict = new Dictionary<int, Pedido>();

            var pedidos = connection.Query<Pedido, DetallePedido, Pedido>(
                "spObtenerHistorialPedidos",
                (pedido, detalle) =>
                {
                    if (!pedidoDict.TryGetValue(pedido.PedidoID, out var ped))
                    {
                        ped = pedido;
                        ped.Detalles = new List<DetallePedido>();
                        pedidoDict.Add(pedido.PedidoID, ped);
                    }
                    ped.Detalles.Add(detalle);
                    return ped;
                },
                param: new { UsuarioID = usuarioId },
                splitOn: "DetallePedidoID",
                commandType: System.Data.CommandType.StoredProcedure
            ).Distinct().ToList();

            return pedidos;
        }

        // Obtener lista de estados de pedido
        public List<EstadoPedido> ObtenerEstadosPedido()
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT EstadoID, NombreEstado FROM EstadoPedido";
            return connection.Query<EstadoPedido>(sql).ToList();
        }

        // Obtener lista de productos con talla y color
        public List<ProductoTallaColor> ObtenerProductosPTC()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                SELECT 
                    ptc.PTCID,
                    ptc.ProductoID,
                    ptc.TallaID,
                    ptc.ColorID,
                    ptc.Stock,
                    prod.Nombre,
                    t.NombreTalla,
                    c.NombreColor
                FROM ProductoTallaColor ptc
                INNER JOIN Producto prod ON ptc.ProductoID = prod.ProductoID
                INNER JOIN Talla t ON ptc.TallaID = t.TallaID
                INNER JOIN Color c ON ptc.ColorID = c.ColorID";

            var resultados = connection.Query<ProductoTallaColorExtended>(sql).ToList();

            return resultados.Select(x => new ProductoTallaColor
            {
                PTCID = x.PTCID,
                ProductoID = x.ProductoID,
                TallaID = x.TallaID,
                ColorID = x.ColorID,
                Stock = x.Stock,
                NombreCompuesto = $"{x.Nombre} - Talla: {x.NombreTalla}, Color: {x.NombreColor}"
            }).ToList();
        }

        public void CrearPedidoConDetalles(int usuarioId, NuevoPedidoInputModel nuevoPedido)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                string sqlInsertPedido = @"
                    INSERT INTO Pedido (UsuarioID, EstadoID)
                    VALUES (@UsuarioID, @EstadoID);
                    SELECT CAST(SCOPE_IDENTITY() AS int);";

                int nuevoPedidoId = connection.QuerySingle<int>(
                    sqlInsertPedido,
                    new { UsuarioID = usuarioId, EstadoID = nuevoPedido.EstadoID },
                    transaction: transaction);

                string sqlInsertDetalle = @"
                    INSERT INTO DetallePedido (PedidoID, PTCID, Cantidad, PrecioUnitario)
                    VALUES (@PedidoID, @PTCID, @Cantidad, @PrecioUnitario);";

                foreach (var detalle in nuevoPedido.Detalles)
                {
                    connection.Execute(
                        sqlInsertDetalle,
                        new
                        {
                            PedidoID = nuevoPedidoId,
                            PTCID = detalle.PTCID,
                            Cantidad = detalle.Cantidad,
                            PrecioUnitario = detalle.PrecioUnitario
                        },
                        transaction: transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private class ProductoTallaColorExtended : ProductoTallaColor
        {
            public string Nombre { get; set; } = string.Empty;
            public string NombreTalla { get; set; } = string.Empty;
            public string NombreColor { get; set; } = string.Empty;
        }
    }
}
*/