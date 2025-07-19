using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ProyectoGrupo1.Services
{
    public class DbService
    {
        private readonly string _connectionString;

        public DbService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TiendaRopaDB");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
