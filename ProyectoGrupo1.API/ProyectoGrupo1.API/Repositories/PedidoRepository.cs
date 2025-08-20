using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoGrupo1.API.DTOs.Pedido;
using ProyectoGrupo1.API.Infra;

namespace ProyectoGrupo1.API.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly IDbConnectionFactory _factory;
        public PedidoRepository(IDbConnectionFactory factory) => _factory = factory;

        public async Task<int> CrearAsync(int usuarioId, IEnumerable<PedidoDetalleCrearDto> detalles)
        {
            using var cn = _factory.Create();
            var table = new DataTable();
            table.Columns.Add("PTCID", typeof(int));
            table.Columns.Add("Cantidad", typeof(int));
            table.Columns.Add("PrecioUnitario", typeof(decimal));

            foreach (var d in detalles)
                table.Rows.Add(d.PTCID, d.Cantidad, d.PrecioUnitario);

            var p = new DynamicParameters();
            p.Add("@UsuarioID", usuarioId);
            p.Add("@Detalles", table.AsTableValuedParameter("dbo.DetallePedidoType"));
            p.Add("@NuevoPedidoID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                await cn.ExecuteAsync("dbo.usp_Pedido_Crear", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@NuevoPedidoID");
            }
            catch (SqlException ex) when (ex.Message.Contains("STOCK_INSUFICIENTE"))
            {
                throw new InvalidOperationException("STOCK_INSUFICIENTE", ex);
            }
        }

        public async Task<IEnumerable<HistorialPedidoDto>> HistorialAsync(int usuarioId)
        {
            using var cn = _factory.Create();
            var data = await cn.QueryAsync<HistorialPedidoDto>(
                "spObtenerHistorialPedidos",
                new { UsuarioID = usuarioId },
                commandType: CommandType.StoredProcedure);
            return data;
        }
    }
}
