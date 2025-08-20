using System.Data;
using Microsoft.Data.SqlClient;

namespace ProyectoGrupo1.Api.Infra
{
    public interface IDbConnectionFactory { IDbConnection Create(); 
    }

    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _cs;
        public SqlConnectionFactory(string cs) => _cs = cs;
        public IDbConnection Create() => new SqlConnection(_cs);
    }
}
