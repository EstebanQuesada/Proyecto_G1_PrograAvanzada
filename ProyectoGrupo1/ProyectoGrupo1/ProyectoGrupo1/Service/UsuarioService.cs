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

            string sql = @"SELECT UsuarioID, Nombre, Correo
                           FROM Usuario
                           WHERE Correo = @Correo AND Contrasena = @Contrasena";

            var parametros = new
            {
                login.Correo,
                Contrasena = HashPassword(login.Contrasena)
            };

            return connection.QueryFirstOrDefault<Usuario>(sql, parametros);
        }

        public bool RegistrarUsuario(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);

            string sql = @"INSERT INTO Usuario (Nombre, Apellido, Correo, Contrasena, FechaRegistro, RolID)
               VALUES (@Nombre, @Apellido, @Correo, @Contrasena, GETDATE(), 1)";


            var parametros = new
            {
                usuario.Nombre,
                usuario.Apellido,
                usuario.Correo,
                Contrasena = HashPassword(usuario.Contrasena)
            };

            try
            {
                connection.Execute(sql, parametros);
                return true;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error SQL: " + ex.Message);
            }
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña no puede ser nula o vacía.");

            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

    }
}
