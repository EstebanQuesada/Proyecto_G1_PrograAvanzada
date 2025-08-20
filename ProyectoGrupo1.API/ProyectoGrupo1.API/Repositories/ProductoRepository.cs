namespace ProyectoGrupo1.API.Repositories;

using System.Data;
using System.Linq;
using Dapper;
using ProyectoGrupo1.API.DTOs.Product;
using ProyectoGrupo1.API.Infra;

public class ProductoRepository : IProductoRepository
{
    private readonly IDbConnectionFactory _factory;
    public ProductoRepository(IDbConnectionFactory factory) => _factory = factory;

    public async Task<IEnumerable<ProductoCatalogoDto>> CatalogoAsync(
        string? busqueda, string? categoria, decimal? precioMin, decimal? precioMax)
    {
        using var cn = _factory.Create();
        return await cn.QueryAsync<ProductoCatalogoDto>(
            "dbo.usp_Producto_Catalogo",
            new { Busqueda = busqueda, Categoria = categoria, PrecioMin = precioMin, PrecioMax = precioMax },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<ProductoDetalleDto?> DetalleAsync(int productoId)
    {
        using var cn = _factory.Create();
        using var multi = await cn.QueryMultipleAsync(
            "dbo.usp_Producto_Detalle_Paquete",
            new { ProductoID = productoId },
            commandType: CommandType.StoredProcedure);

        var detalle = await multi.ReadFirstOrDefaultAsync<ProductoDetalleDto>();
        if (detalle is null) return null;

        detalle.Imagenes = (await multi.ReadAsync<string>()).ToList();
        detalle.Tallas = (await multi.ReadAsync<string>()).ToList();
        detalle.Colores = (await multi.ReadAsync<string>()).ToList();
        detalle.PTCs = (await multi.ReadAsync<ProductoTallaColorDto>()).ToList();
        return detalle;
    }

    public async Task<IEnumerable<string>> CategoriasAsync()
    {
        using var cn = _factory.Create();
        return await cn.QueryAsync<string>(
            "dbo.usp_Producto_Categorias",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<ProductoTallaColorDto?> PtcPorIdAsync(int ptcId)
    {
        using var cn = _factory.Create();
        return await cn.QueryFirstOrDefaultAsync<ProductoTallaColorDto>(
            "dbo.usp_Producto_PTC_PorId",
            new { PTCID = ptcId },
            commandType: CommandType.StoredProcedure);
    }
}
