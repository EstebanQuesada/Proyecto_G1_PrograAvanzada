namespace ProyectoGrupo1.API.Repositories;

using ProyectoGrupo1.API.DTOs.Product;

public interface IProductoRepository
{
    Task<IEnumerable<ProductoCatalogoDto>> CatalogoAsync(string? busqueda, string? categoria, decimal? precioMin, decimal? precioMax);
    Task<ProductoDetalleDto?> DetalleAsync(int productoId);
    Task<IEnumerable<string>> CategoriasAsync();
    Task<ProductoTallaColorDto?> PtcPorIdAsync(int ptcId);
}
