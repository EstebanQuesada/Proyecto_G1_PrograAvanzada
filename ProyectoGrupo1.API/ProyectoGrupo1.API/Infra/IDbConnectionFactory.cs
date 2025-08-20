namespace ProyectoGrupo1.API.Infra;

using System.Data;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}
