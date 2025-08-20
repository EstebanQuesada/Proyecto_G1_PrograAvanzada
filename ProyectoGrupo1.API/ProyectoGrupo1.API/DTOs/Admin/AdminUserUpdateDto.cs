namespace ProyectoGrupo1.Api.DTOs.Admin
{
    public record AdminUserUpdateDto(
        int UsuarioID, string Nombre, string Apellido, string Correo,
        int RolID, string Direccion, string Ciudad, string Provincia, string CodigoPostal);
}