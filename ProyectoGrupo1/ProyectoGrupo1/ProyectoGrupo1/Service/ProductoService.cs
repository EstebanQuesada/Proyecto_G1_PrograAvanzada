namespace ProyectoGrupo1.Service
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Dapper;
    using Microsoft.Extensions.Configuration;
    using ProyectoGrupo1.Models;

    public class ProductoService
    {
        private readonly string _connectionString;

        public ProductoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TiendaRopaDB");
        }


        public ProductoDetalleViewModel ObtenerDetalleProducto(int productoId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlProducto = @"
            SELECT 
                p.ProductoID,
                p.Nombre,
                p.Descripcion,
                p.Precio,
                c.NombreCategoria AS Categoria
            FROM Producto p
            INNER JOIN CategoriaProducto c ON p.CategoriaID = c.CategoriaID
            WHERE p.ProductoID = @ProductoID";

                var producto = connection.QueryFirstOrDefault<ProductoDetalleViewModel>(sqlProducto, new { ProductoID = productoId });

                if (producto == null) return null;

                var sqlImagenes = "SELECT UrlImagen FROM ImagenProducto WHERE ProductoID = @ProductoID";
                producto.Imagenes = connection.Query<string>(sqlImagenes, new { ProductoID = productoId }).ToList();

                var sqlTallas = @"
            SELECT DISTINCT t.NombreTalla
            FROM ProductoTallaColor ptc
            INNER JOIN Talla t ON ptc.TallaID = t.TallaID
            WHERE ptc.ProductoID = @ProductoID";
                producto.Tallas = connection.Query<string>(sqlTallas, new { ProductoID = productoId }).ToList();

                var sqlColores = @"
            SELECT DISTINCT c.NombreColor
            FROM ProductoTallaColor ptc
            INNER JOIN Color c ON ptc.ColorID = c.ColorID
            WHERE ptc.ProductoID = @ProductoID";
                producto.Colores = connection.Query<string>(sqlColores, new { ProductoID = productoId }).ToList();

                return producto;
            }
        }
        public List<ProductoCatalogoViewModel> ObtenerProductosCatalogo()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
            SELECT 
                p.ProductoID,
                p.Nombre,
                LEFT(p.Descripcion, 100) AS DescripcionCorta,
                p.Precio,
                c.NombreCategoria AS Categoria,
                (
                    SELECT TOP 1 UrlImagen 
                    FROM ImagenProducto 
                    WHERE ProductoID = p.ProductoID
                ) AS UrlImagenPrincipal
            FROM Producto p
            INNER JOIN CategoriaProducto c ON p.CategoriaID = c.CategoriaID";

                return connection.Query<ProductoCatalogoViewModel>(sql).ToList();
            }
        }
        public List<string> ObtenerCategorias()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT NombreCategoria FROM CategoriaProducto";
                return connection.Query<string>(sql).ToList();
            }
        }


    }

}
