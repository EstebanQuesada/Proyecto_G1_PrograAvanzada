using ProyectoGrupo1.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;

namespace ProyectoGrupo1.Service
{
    public class CarritoService
    {
        private readonly string _connectionString;

        public CarritoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TiendaRopaDB");
        }

        public Carrito ObtenerCarritoPorUsuario(int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            var carrito = connection.QueryFirstOrDefault<Carrito>(
                "SELECT TOP 1 * FROM Carrito WHERE UsuarioID = @UsuarioID ORDER BY FechaCreacion DESC",
                new { UsuarioID = usuarioId });
            if (carrito == null)
            {
                // Crear carrito si no existe
                int carritoId = connection.QuerySingle<int>(
                    "INSERT INTO Carrito (UsuarioID, FechaCreacion) VALUES (@UsuarioID, GETDATE()); SELECT CAST(SCOPE_IDENTITY() as int);",
                    new { UsuarioID = usuarioId });
                carrito = new Carrito { CarritoID = carritoId, UsuarioID = usuarioId, FechaCreacion = DateTime.Now, Detalles = new List<DetalleCarrito>() };
            }
            else
            {
                var detalles = connection.Query<DetalleCarrito>(
                    @"SELECT dc.DetalleID, dc.CarritoID, dc.PTCID, dc.Cantidad,
                             p.Nombre AS NombreProducto, ip.UrlImagen, t.NombreTalla, c.NombreColor, p.Precio AS PrecioUnitario
                      FROM DetalleCarrito dc
                      INNER JOIN ProductoTallaColor ptc ON dc.PTCID = ptc.PTCID
                      INNER JOIN Producto p ON ptc.ProductoID = p.ProductoID
                      LEFT JOIN ImagenProducto ip ON p.ProductoID = ip.ProductoID AND ip.ImagenID = (SELECT MIN(ImagenID) FROM ImagenProducto WHERE ProductoID = p.ProductoID)
                      INNER JOIN Talla t ON ptc.TallaID = t.TallaID
                      INNER JOIN Color c ON ptc.ColorID = c.ColorID
                      WHERE dc.CarritoID = @CarritoID",
                    new { CarritoID = carrito.CarritoID }).ToList();
                carrito.Detalles = detalles;
            }
            return carrito;
        }

        public void AgregarOActualizarProducto(int usuarioId, int ptcId, int cantidad)
        {
            using var connection = new SqlConnection(_connectionString);
            var carrito = ObtenerCarritoPorUsuario(usuarioId);
            var detalle = connection.QueryFirstOrDefault<DetalleCarrito>(
                "SELECT * FROM DetalleCarrito WHERE CarritoID = @CarritoID AND PTCID = @PTCID",
                new { CarritoID = carrito.CarritoID, PTCID = ptcId });
            if (detalle == null)
            {
                connection.Execute(
                    "INSERT INTO DetalleCarrito (CarritoID, PTCID, Cantidad) VALUES (@CarritoID, @PTCID, @Cantidad)",
                    new { CarritoID = carrito.CarritoID, PTCID = ptcId, Cantidad = cantidad });
            }
            else
            {
                connection.Execute(
                    "UPDATE DetalleCarrito SET Cantidad = @Cantidad WHERE DetalleID = @DetalleID",
                    new { Cantidad = cantidad, DetalleID = detalle.DetalleID });
            }
        }

        public void EliminarProducto(int usuarioId, int ptcId)
        {
            using var connection = new SqlConnection(_connectionString);
            var carrito = ObtenerCarritoPorUsuario(usuarioId);
            connection.Execute(
                "DELETE FROM DetalleCarrito WHERE CarritoID = @CarritoID AND PTCID = @PTCID",
                new { CarritoID = carrito.CarritoID, PTCID = ptcId });
        }

        public void VaciarCarrito(int usuarioId)
        {
            using var connection = new SqlConnection(_connectionString);
            var carrito = ObtenerCarritoPorUsuario(usuarioId);
            connection.Execute(
                "DELETE FROM DetalleCarrito WHERE CarritoID = @CarritoID",
                new { CarritoID = carrito.CarritoID });
        }
    }
} 