namespace ProyectoGrupo1.Api.DTOs
{
    public record RegisterDto(
        string Nombre, string Apellido, string Correo, string Contrasena,
        string Direccion, string Ciudad, string Provincia, string CodigoPostal);
}
