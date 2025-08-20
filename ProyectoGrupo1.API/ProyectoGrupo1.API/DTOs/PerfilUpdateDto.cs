namespace ProyectoGrupo1.Api.DTOs
{
    public record PerfilUpdateDto(
        int UsuarioID, string Nombre, string Apellido, string Correo,
        string Direccion, string Ciudad, string Provincia, string CodigoPostal);
}
