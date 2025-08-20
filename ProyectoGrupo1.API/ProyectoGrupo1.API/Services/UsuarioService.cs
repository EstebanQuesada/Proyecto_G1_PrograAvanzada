using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using ProyectoGrupo1.Api.Infra;
using ProyectoGrupo1.Api.Models;

namespace ProyectoGrupo1.Api.Services
{
    public class UsuarioService
    {
        private readonly IDbConnectionFactory _factory;
        public UsuarioService(IDbConnectionFactory factory) => _factory = factory;

        private static string HashPassword(string? password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password ?? "");
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }

        public async Task<Usuario?> ValidarUsuarioAsync(string correo, string contrasena)
        {
            try
            {
                using var con = _factory.Create();
                var p = new DynamicParameters();
                p.Add("@Correo", correo);
                p.Add("@ContrasenaHash", HashPassword(contrasena));

                var u = await con.QueryFirstOrDefaultAsync<Usuario>(
                    "sp_Usuario_Validar", p, commandType: CommandType.StoredProcedure);

                if (u is null) return null;                

                if (!u.Activo)
                    throw new AppException("Tu cuenta está inactiva.", 403);

                if (u.Bloqueado)
                    
                    throw new AppException("Tu cuenta está bloqueada.", 423);

                return u;
            }
            catch (SqlException ex)
            {
                throw new AppException("Error al validar usuario.", 500, ex);
            }
        }


        public async Task<int> RegistrarUsuarioAsync(Usuario u)
        {
            try
            {
                using var con = _factory.Create();
                var p = new DynamicParameters();
                p.Add("@Nombre", u.Nombre);
                p.Add("@Apellido", u.Apellido);
                p.Add("@Correo", u.Correo);
                p.Add("@ContrasenaHash", HashPassword(u.Contrasena));
                p.Add("@Direccion", u.Direccion);
                p.Add("@Ciudad", u.Ciudad);
                p.Add("@Provincia", u.Provincia);
                p.Add("@CodigoPostal", u.CodigoPostal);
                p.Add("@NuevoUsuarioID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await con.ExecuteAsync("sp_Usuario_Registrar", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@NuevoUsuarioID");
            }
            catch (SqlException ex) when (ex.Number == 2627 )
            {
                throw new AppException("El correo ya está registrado.", 409, ex);
            }
            catch (Exception ex)
            {
                throw new AppException("Error al registrar usuario.", 500, ex);
            }
        }

        public async Task<Usuario?> ObtenerPerfilCompletoAsync(int id)
        {
            try
            {
                using var con = _factory.Create();
                return await con.QueryFirstOrDefaultAsync<Usuario>(
                    "sp_Usuario_ObtenerPerfil",
                    new { UsuarioID = id },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new AppException("Error al obtener el perfil.", 500, ex);
            }
        }

        public async Task<bool> ActualizarPerfilYDireccionAsync(Usuario u)
        {
            try
            {
                using var con = _factory.Create();
                var p = new DynamicParameters();
                p.Add("@UsuarioID", u.UsuarioID);
                p.Add("@Nombre", u.Nombre);
                p.Add("@Apellido", u.Apellido);
                p.Add("@Correo", u.Correo);
                p.Add("@Direccion", u.Direccion);
                p.Add("@Ciudad", u.Ciudad);
                p.Add("@Provincia", u.Provincia);
                p.Add("@CodigoPostal", u.CodigoPostal);

                var rows = await con.ExecuteAsync("sp_Usuario_ActualizarPerfilYDireccion",
                    p, commandType: CommandType.StoredProcedure);

                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new AppException("Error al actualizar el perfil.", 500, ex);
            }
        }

        public async Task<bool> CambiarContrasenaAsync(int usuarioId, string actual, string nueva)
        {
            try
            {
                using var con = _factory.Create();

                var hashActual = await con.ExecuteScalarAsync<string>(
                    "sp_Usuario_ObtenerHashContrasena",
                    new { UsuarioID = usuarioId },
                    commandType: CommandType.StoredProcedure);

                if (!string.Equals(hashActual, HashPassword(actual), StringComparison.Ordinal))
                    return false;

                var rows = await con.ExecuteAsync(
                    "sp_Usuario_CambiarContrasena",
                    new { UsuarioID = usuarioId, NuevaContrasenaHash = HashPassword(nueva) },
                    commandType: CommandType.StoredProcedure);

                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new AppException("Error al cambiar la contraseña.", 500, ex);
            }
        }
    }
}
