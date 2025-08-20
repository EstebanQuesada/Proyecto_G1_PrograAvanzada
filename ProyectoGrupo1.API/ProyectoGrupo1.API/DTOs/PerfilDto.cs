namespace ProyectoGrupo1.Api.DTOs
{
    public record PerfilDto(
        int UsuarioID, string Nombre, string Apellido, string Correo,
        string Direccion, string Ciudad, string Provincia, string CodigoPostal, int RolID);
}
