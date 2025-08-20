using Microsoft.AspNetCore.Mvc;
using ProyectoGrupo1.API.DTOs.Product;

namespace ProyectoGrupo1.Api.Repositories
{
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Dapper;
    using ProyectoGrupo1.Api.Infra;
    using ProyectoGrupo1.API.Repositories;

    public class ProductoRepository : IProductoRepository
    {
        private readonly IDbConnectionFactory _factory;
        public ProductoRepository(IDbConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<ProductoCatalogoDto>> CatalogoAsync(
            string? busqueda, string? categoria, decimal? precioMin, decimal? precioMax)
        {
            using var cn = _factory.Create();
            if (cn.State != ConnectionState.Open) await ((dynamic)cn).OpenAsync(); 
            return await cn.QueryAsync<ProductoCatalogoDto>(
                "dbo.usp_Producto_Catalogo",
                new { Busqueda = busqueda, Categoria = categoria, PrecioMin = precioMin, PrecioMax = precioMax },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<ProductoDetalleDto?> DetalleAsync(int productoId)
        {
            using var cn = _factory.Create();
            if (cn.State != ConnectionState.Open) await ((dynamic)cn).OpenAsync();

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
            if (cn.State != ConnectionState.Open) await ((dynamic)cn).OpenAsync();
            return await cn.QueryAsync<string>("dbo.usp_Producto_Categorias", commandType: CommandType.StoredProcedure);
        }
        public async Task<ProductoTallaColorDto?> PtcPorIdAsync(int ptcId)
        {
            const string sql = @"
            SELECT ptc.PTCID, ptc.ProductoID, ptc.TallaID, ptc.ColorID, ptc.Stock,
                   t.NombreTalla, c.NombreColor
            FROM ProductoTallaColor ptc
            INNER JOIN Talla  t ON t.TallaID  = ptc.TallaID
            INNER JOIN Color  c ON c.ColorID  = ptc.ColorID
            WHERE ptc.PTCID = @ptcId;
        ";

            using var cn = _factory.Create();
            return await cn.QueryFirstOrDefaultAsync<ProductoTallaColorDto>(sql, new { ptcId });
        }
    }
}

