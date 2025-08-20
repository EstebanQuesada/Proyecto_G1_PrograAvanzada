namespace ProyectoGrupo1.Api.Models;

public record UsuarioListItem(
    int UsuarioID,
    string Nombre,
    string Apellido,
    string Correo,
    int RolID,
    DateTime? FechaRegistro,
    bool Bloqueado,
    bool Activo);
