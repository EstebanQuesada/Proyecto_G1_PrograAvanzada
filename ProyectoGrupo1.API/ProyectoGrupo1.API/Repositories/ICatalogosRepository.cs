using ProyectoGrupo1.API.DTOs;

namespace ProyectoGrupo1.API.Repositories
{
    public interface ICatalogosRepository
    {
        Task<IEnumerable<LookupItemDto>> ListarCategoriasAsync();
        Task<int> CrearCategoriaAsync(string nombre);
        Task<bool> ActualizarCategoriaAsync(int id, string nombre);
        Task<bool> EliminarCategoriaAsync(int id);

        Task<IEnumerable<LookupItemDto>> ListarMarcasAsync();
        Task<int> CrearMarcaAsync(string nombre);
        Task<bool> ActualizarMarcaAsync(int id, string nombre);
        Task<bool> EliminarMarcaAsync(int id);

        Task<IEnumerable<LookupItemDto>> ListarTallasAsync();
        Task<int> CrearTallaAsync(string nombre);
        Task<bool> ActualizarTallaAsync(int id, string nombre);
        Task<bool> EliminarTallaAsync(int id);

        Task<IEnumerable<LookupItemDto>> ListarColoresAsync();
        Task<int> CrearColorAsync(string nombre);
        Task<bool> ActualizarColorAsync(int id, string nombre);
        Task<bool> EliminarColorAsync(int id);

        Task<IEnumerable<ProveedorDto>> ListarProveedoresAsync();
        Task<int> CrearProveedorAsync(ProveedorSaveDto dto);
        Task<bool> ActualizarProveedorAsync(int id, ProveedorSaveDto dto);
        Task<bool> EliminarProveedorAsync(int id);
    }
}
