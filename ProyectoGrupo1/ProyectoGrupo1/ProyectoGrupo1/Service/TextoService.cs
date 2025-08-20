using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace ProyectoGrupo1.Services
{
    public class TextoService
    {
        private readonly DbService _dbService;

        public TextoService(DbService dbService)
        {
            _dbService = dbService;
        }

        public string ObtenerDescripcionContacto()
        {
            using var connection = _dbService.CreateConnection();

            return connection.ExecuteScalar<string>(
                "ObtenerDescripcionContacto",
                commandType: CommandType.StoredProcedure);
        }
    }
}
