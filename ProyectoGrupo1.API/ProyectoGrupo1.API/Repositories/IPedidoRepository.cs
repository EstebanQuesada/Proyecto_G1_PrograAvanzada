namespace ProyectoGrupo1.API.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ProyectoGrupo1.API.DTOs.Pedido;

    public interface IPedidoRepository
    {
        Task<int> CrearAsync(int usuarioId, IEnumerable<PedidoDetalleCrearDto> detalles);
        Task<IEnumerable<HistorialPedidoDto>> HistorialAsync(int usuarioId);
    }
}
