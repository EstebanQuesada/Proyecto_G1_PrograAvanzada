using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoGrupo1.Api.Infra;
using ProyectoGrupo1.Api.Models;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoGrupo1.Api.Services
{
    public class AdminUsuarioService
    {
        private readonly IDbConnectionFactory _factory;
        public AdminUsuarioService(IDbConnectionFactory factory) => _factory = factory;

        public async Task<(int Total, IEnumerable<UsuarioListItem> Items)> ListarAsync(int page, int pageSize, string? search)
        {
            try
            {
                using var con = _factory.Create();
                var p = new DynamicParameters();
                p.Add("@Page", page);
                p.Add("@PageSize", pageSize);
                p.Add("@Search", search);

                using var multi = await con.QueryMultipleAsync(
                    "sp_Admin_Usuario_Listar", p, commandType: CommandType.StoredProcedure);

                var total = await multi.ReadFirstAsync<int>();
                var items = await multi.ReadAsync<UsuarioListItem>();
                return (total, items);
            }
            catch (SqlException ex)
            {
                throw new AppException("Error al listar usuarios.", 500, ex);
            }
        }

        public async Task<Usuario?> ObtenerAsync(int id)
        {
            try
            {
                using var con = _factory.Create();
                return await con.QueryFirstOrDefaultAsync<Usuario>(
                    "sp_Admin_Usuario_Obtener",
                    new { UsuarioID = id },
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                throw new AppException("Error al obtener el usuario.", 500, ex);
            }
        }

        public async Task<int> CrearAsync(Usuario u)
        {
            try
            {
                using var con = _factory.Create();
                var p = new DynamicParameters();
                p.Add("@Nombre", u.Nombre);
                p.Add("@Apellido", u.Apellido);
                p.Add("@Correo", u.Correo);
                p.Add("@ContrasenaHash", Hash(u.Contrasena));
                p.Add("@RolID", u.RolID);
                p.Add("@Direccion", u.Direccion);
                p.Add("@Ciudad", u.Ciudad);
                p.Add("@Provincia", u.Provincia);
                p.Add("@CodigoPostal", u.CodigoPostal);
                p.Add("@NuevoUsuarioID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await con.ExecuteAsync("sp_Admin_Usuario_Crear", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@NuevoUsuarioID");
            }
            catch (SqlException ex)
            {
                throw new AppException("Error al crear el usuario.", 500, ex);
            }
        }
        public async Task<bool> ActualizarAsync(Usuario u)
        {
            try
            {
                using var con = _factory.Create();
                var rows = await con.ExecuteAsync(
                    "sp_Admin_Usuario_Actualizar",
                    new
                    {
                        u.UsuarioID,
                        u.Nombre,
                        u.Apellido,
                        u.Correo,
                        u.RolID,
                        u.Direccion,
                        u.Ciudad,
                        u.Provincia,
                        u.CodigoPostal
                    },
                    commandType: CommandType.StoredProcedure);

                return rows > 0;
            }
            catch (SqlException ex)
            {
                throw new AppException("Error al actualizar el usuario.", 500, ex);
            }
        }

        public async Task<bool> CambiarRolAsync(int id, int rolId)
        {
            try
            {
                using var con = _factory.Create();
                var rows = await con.ExecuteAsync(
                    "sp_Admin_Usuario_CambiarRol",
                    new { UsuarioID = id, RolID = rolId },
                    commandType: CommandType.StoredProcedure);

                return rows > 0;
            }
            catch (SqlException ex)
            {
                throw new AppException("Error al cambiar el rol.", 500, ex);
            }
        }

        public async Task<bool> BloquearAsync(int id, bool bloqueado)
        {
            try
            {
                using var con = _factory.Create();
                var rows = await con.ExecuteAsync(
                    "sp_Admin_Usuario_Bloquear",
                    new { UsuarioID = id, Bloqueado = bloqueado },
                    commandType: CommandType.StoredProcedure);

                return rows > 0;
            }
            catch (SqlException ex)
            {
                throw new AppException("Error al cambiar el bloqueo.", 500, ex);
            }
        }

        public async Task<bool> ResetPasswordAsync(int id, string nueva)
        {
            try
            {
                using var con = _factory.Create();
                var rows = await con.ExecuteAsync(
                    "sp_Admin_Usuario_ResetPassword",
                    new { UsuarioID = id, NuevaContrasenaHash = Hash(nueva) },
                    commandType: CommandType.StoredProcedure);

                return rows > 0;
            }
            catch (SqlException ex)
            {
                throw new AppException("Error al reiniciar la contraseña.", 500, ex);
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                using var con = _factory.Create();
                var rows = await con.ExecuteAsync(
                    "sp_Admin_Usuario_Eliminar",
                    new { UsuarioID = id },
                    commandType: CommandType.StoredProcedure);

                return rows > 0;
            }
            catch (SqlException ex)
            {
                throw new AppException("Error al eliminar (soft) el usuario.", 500, ex);
            }
        }

        public async Task<bool> RestaurarAsync(int id)
        {
            try
            {
                using var con = _factory.Create();
                var rows = await con.ExecuteAsync(
                    "sp_Admin_Usuario_Restaurar",
                    new { UsuarioID = id },
                    commandType: CommandType.StoredProcedure);

                return rows > 0;
            }
            catch (SqlException ex)
            {
                throw new AppException("Error al restaurar el usuario.", 500, ex);
            }
        }

        private static string Hash(string? s)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(s ?? string.Empty);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }
    }

    public record UsuarioListItem(
        int UsuarioID,
        string Nombre,
        string Apellido,
        string Correo,
        int RolID,
        DateTime? FechaRegistro,
        bool Bloqueado,
        bool Activo);
}
