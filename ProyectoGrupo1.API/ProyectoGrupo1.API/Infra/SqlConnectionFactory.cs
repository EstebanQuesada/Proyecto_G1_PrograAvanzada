namespace ProyectoGrupo1.API.Infra;

using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _cs;

    public SqlConnectionFactory(IConfiguration cfg)
    {
        _cs = cfg.GetConnectionString("TiendaRopaDB")
              ?? cfg.GetConnectionString("Default")
              ?? throw new InvalidOperationException(
                   "Falta ConnectionStrings:TiendaRopaDB o ConnectionStrings:Default");
    }

    public IDbConnection Create() => new SqlConnection(_cs);
}
