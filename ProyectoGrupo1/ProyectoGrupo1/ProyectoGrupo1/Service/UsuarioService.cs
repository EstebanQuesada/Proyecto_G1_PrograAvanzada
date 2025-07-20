using ProyectoGrupo1.Models;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoGrupo1.Services
{
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TiendaRopaDB")
                ?? throw new Exception("No se encontró la cadena de conexión 'TiendaRopaDB'");
        }

        public Usuario ValidarUsuario(Usuario login)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"SELECT UsuarioID, Nombre, Correo FROM Usuario
                           WHERE Correo = @Correo AND Contrasena = @Contrasena";

            return connection.QueryFirstOrDefault<Usuario>(sql, new
            {
                login.Correo,
                Contrasena = HashPassword(login.Contrasena)
            });
        }

        public bool RegistrarUsuario(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);

            string sqlUsuario = @"
        INSERT INTO Usuario (Nombre, Apellido, Correo, Contrasena, FechaRegistro, RolID)
        VALUES (@Nombre, @Apellido, @Correo, @Contrasena, GETDATE(), 1);
        SELECT CAST(SCOPE_IDENTITY() as int);";

            int usuarioId = connection.QuerySingle<int>(sqlUsuario, new
            {
                usuario.Nombre,
                usuario.Apellido,
                usuario.Correo,
                Contrasena = HashPassword(usuario.Contrasena)
            });

            string sqlDireccion = @"
        INSERT INTO DireccionUsuario (UsuarioID, Direccion, Ciudad, Provincia, CodigoPostal)
        VALUES (@UsuarioID, @Direccion, @Ciudad, @Provincia, @CodigoPostal);";

            connection.Execute(sqlDireccion, new
            {
                UsuarioID = usuarioId,
                usuario.Direccion,
                usuario.Ciudad,
                usuario.Provincia,
                usuario.CodigoPostal
            });

            return usuarioId > 0;
        }


        public Usuario ObtenerPerfilCompleto(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var usuario = connection.QueryFirstOrDefault<Usuario>("SELECT * FROM Usuario WHERE UsuarioID = @id", new { id });
            var direccion = connection.QueryFirstOrDefault<DireccionUsuario>("SELECT * FROM DireccionUsuario WHERE UsuarioID = @id", new { id });

            if (usuario != null && direccion != null)
            {
                usuario.Direccion = direccion.Direccion;
                usuario.Ciudad = direccion.Ciudad;
                usuario.Provincia = direccion.Provincia;
                usuario.CodigoPostal = direccion.CodigoPostal;
            }

            return usuario;
        }

        public bool ActualizarPerfilYDireccion(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);

            string sqlUsuario = @"UPDATE Usuario 
                                  SET Nombre = @Nombre, Apellido = @Apellido, Correo = @Correo
                                  WHERE UsuarioID = @UsuarioID";

            string sqlDireccion = @"UPDATE DireccionUsuario
                                    SET Direccion = @Direccion, Ciudad = @Ciudad,
                                        Provincia = @Provincia, CodigoPostal = @CodigoPostal
                                    WHERE UsuarioID = @UsuarioID";

            var rows1 = connection.Execute(sqlUsuario, usuario);
            var rows2 = connection.Execute(sqlDireccion, usuario);

            return rows1 > 0 || rows2 > 0;
        }

        public bool CambiarContrasena(int usuarioId, string actual, string nueva)
        {
            using var connection = new SqlConnection(_connectionString);

            string actualHash = connection.QueryFirstOrDefault<string>(
                "SELECT Contrasena FROM Usuario WHERE UsuarioID = @usuarioId", new { usuarioId });

            if (actualHash != HashPassword(actual))
                return false;

            int rows = connection.Execute("UPDATE Usuario SET Contrasena = @Nueva WHERE UsuarioID = @usuarioId", new
            {
                Nueva = HashPassword(nueva),
                usuarioId
            });

            return rows > 0;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }
    }
}
