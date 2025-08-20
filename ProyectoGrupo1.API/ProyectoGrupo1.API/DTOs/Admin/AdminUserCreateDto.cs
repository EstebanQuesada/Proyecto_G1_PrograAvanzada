namespace ProyectoGrupo1.Api.DTOs.Admin
{
    public record AdminUserCreateDto(
        string Nombre, string Apellido, string Correo, string Contrasena,
        int RolID, string Direccion, string Ciudad, string Provincia, string CodigoPostal);
}