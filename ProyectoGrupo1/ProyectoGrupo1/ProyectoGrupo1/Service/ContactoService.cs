using ProyectoGrupo1.Models;
using System.Data;
using Dapper;

namespace ProyectoGrupo1.Services
{
    public class ContactoService
    {
        private readonly DbService _dbService;

        public ContactoService(DbService dbService)
        {
            _dbService = dbService;
        }

        public void GuardarMensaje(ContactoViewModel modelo)
        {
            using IDbConnection connection = _dbService.CreateConnection();

            connection.Execute(
                "SP_GuardarContacto",
                new
                {
                    Nombre = modelo.Nombre,
                    Correo = modelo.Correo,
                    Mensaje = modelo.Mensaje,
                    FechaEnvio = DateTime.Now
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

