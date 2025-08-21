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
    }

    public class ContactoRepository : IContactoRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public ContactoRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<int> CrearAsync(ContactoCrearDto dto)
        {
            using var conn = _dbFactory.Create();
            return await conn.ExecuteAsync(
                "SP_GuardarContacto",
                new { dto.Nombre, dto.Correo, dto.Mensaje, FechaEnvio = DateTime.Now },
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
    }
}

