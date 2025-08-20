using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoGrupo1.API.DTOs.Product;

namespace ProyectoGrupo1.API.Repositories
{

    public interface IProductoRepository
    {
        Task<IEnumerable<ProductoCatalogoDto>> CatalogoAsync(string? busqueda, string? categoria, decimal? precioMin, decimal? precioMax);
        Task<ProductoDetalleDto?> DetalleAsync(int productoId);
        Task<IEnumerable<string>> CategoriasAsync();
        Task<ProductoTallaColorDto?> PtcPorIdAsync(int ptcId);

    }
}
