
using Dapper;
using ProyectoGrupo1.API.DTOs.Contacto;
using ProyectoGrupo1.API.Models;
using ProyectoGrupo1.API.Infra;
using System.Data;

namespace ProyectoGrupo1.API.Repositories
{
    public interface IContactoRepository
    {
        Task<int> CrearAsync(ContactoCrearDto dto);
        Task<IEnumerable<ContactoMensaje>> ListarAsync();

        Task<(IEnumerable<ContactoMensaje> Items, int Total)> ListarAdminAsync(string? buscar, string? estado, int page, int pageSize);
        Task<int> ActualizarEstadoAsync(int id, bool visto, bool completado);
    }

    public class ContactoRepository : IContactoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public ContactoRepository(IDbConnectionFactory dbFactory) => _dbFactory = dbFactory;

        public async Task<int> CrearAsync(ContactoCrearDto dto)
        {
            using var conn = _dbFactory.Create();
            return await conn.ExecuteAsync(
                "SP_GuardarContacto",
                new { dto.Nombre, dto.Correo, dto.Mensaje, FechaEnvio = DateTime.UtcNow },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ContactoMensaje>> ListarAsync()
        {
            using var conn = _dbFactory.Create();
            return await conn.QueryAsync<ContactoMensaje>(
                "SP_ListarContactos",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<(IEnumerable<ContactoMensaje> Items, int Total)> ListarAdminAsync(string? buscar, string? estado, int page, int pageSize)
        {
            using var conn = _dbFactory.Create();
            var p = new DynamicParameters();
            p.Add("@Buscar", buscar);
            p.Add("@Estado", estado); 
            p.Add("@Page", page);
            p.Add("@PageSize", pageSize);
            p.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var items = await conn.QueryAsync<ContactoMensaje>(
                "SP_Contacto_ListarAdmin",
                p,
                commandType: CommandType.StoredProcedure
            );

            var total = p.Get<int>("@Total");
            return (items, total);
        }

        public async Task<int> ActualizarEstadoAsync(int id, bool visto, bool completado)
        {
            using var conn = _dbFactory.Create();
            return await conn.ExecuteAsync(
                "SP_Contacto_ActualizarEstado",
                new { Id = id, Visto = visto, Completado = completado, Fecha = DateTime.UtcNow },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
