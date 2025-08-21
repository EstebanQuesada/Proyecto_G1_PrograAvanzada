using ProyectoGrupo1.API.DTOs.Contacto;
using ProyectoGrupo1.API.Models;
using ProyectoGrupo1.API.Repositories;

namespace ProyectoGrupo1.API.Services
{
    public interface IContactoService
    {
        Task<int> CrearMensajeAsync(ContactoCrearDto dto);
        Task<IEnumerable<ContactoMensaje>> ListarMensajesAsync();

        Task<(IEnumerable<ContactoMensaje> Items, int Total)> ListarAdminAsync(string? buscar, string? estado, int page, int pageSize);
        Task<int> ActualizarEstadoAsync(int id, bool visto, bool completado);
    }

    public class ContactoService : IContactoService
    {
        private readonly IContactoRepository _repo;
        public ContactoService(IContactoRepository repo) => _repo = repo;

        public Task<int> CrearMensajeAsync(ContactoCrearDto dto) => _repo.CrearAsync(dto);
        public Task<IEnumerable<ContactoMensaje>> ListarMensajesAsync() => _repo.ListarAsync();

        public Task<(IEnumerable<ContactoMensaje> Items, int Total)> ListarAdminAsync(string? buscar, string? estado, int page, int pageSize)
            => _repo.ListarAdminAsync(buscar, estado, page, pageSize);

        public Task<int> ActualizarEstadoAsync(int id, bool visto, bool completado)
            => _repo.ActualizarEstadoAsync(id, visto, completado);
    }
}