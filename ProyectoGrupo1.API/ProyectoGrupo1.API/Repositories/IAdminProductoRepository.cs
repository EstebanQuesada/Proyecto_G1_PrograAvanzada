using ProyectoGrupo1.API.DTOs.ProductAdmin;

namespace ProyectoGrupo1.API.Repositories
{
    public interface IAdminProductoRepository
    {
        Task<(int Total, IEnumerable<AdminProductoListItemDto> Items)> ListarAsync(
            int page, int pageSize, string? search);

        Task<(AdminProductoDto? Producto, AdminLookupsDto Lookups)> ObtenerAsync(int id);

        Task<int> CrearAsync(AdminProductoSaveDto dto);

        Task<bool> ActualizarAsync(int id, AdminProductoSaveDto dto);

        Task<bool> EliminarAsync(int id);
    }
}
