using ProyectoGrupo1.API.DTOs.Contacto;
using ProyectoGrupo1.API.Models;
using ProyectoGrupo1.API.Repositories;

namespace ProyectoGrupo1.API.Services
{
    public interface IContactoService
    {
        Task<int> CrearMensajeAsync(ContactoCrearDto dto);
        Task<IEnumerable<ContactoMensaje>> ListarMensajesAsync();
    }

    public class ContactoService : IContactoService
    {
        private readonly IContactoRepository _repo;

        public ContactoService(IContactoRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> CrearMensajeAsync(ContactoCrearDto dto)
        {
            return await _repo.CrearAsync(dto);
        }

        public async Task<IEnumerable<ContactoMensaje>> ListarMensajesAsync()
        {
            return await _repo.ListarAsync();
        }
    }
}


