namespace ProyectoGrupo1.Api.DTOs.Admin
{
    public record AdminUserListItemDto(
        int UsuarioID, string Nombre, string Apellido, string Correo,
        int RolID, DateTime? FechaRegistro, bool Bloqueado, bool Activo);
}