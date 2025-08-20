using System.ComponentModel.DataAnnotations;

namespace ProyectoGrupo1.Models
{
    public class AdminCrearVm
    {
        [Required] public string Nombre { get; set; } = "";
        [Required] public string Apellido { get; set; } = "";
        [Required, EmailAddress] public string Correo { get; set; } = "";
        [Required, MinLength(6)] public string Contrasena { get; set; } = "";
        [Range(1, int.MaxValue)] public int RolID { get; set; } = 1; 
        [Required] public string Direccion { get; set; } = "";
        [Required] public string Ciudad { get; set; } = "";
        [Required] public string Provincia { get; set; } = "";
        [Required] public string CodigoPostal { get; set; } = "";
    }

    public class AdminEditarVm
    {
        [Required] public int UsuarioID { get; set; }
        [Required] public string Nombre { get; set; } = "";
        [Required] public string Apellido { get; set; } = "";
        [Required, EmailAddress] public string Correo { get; set; } = "";
        [Range(1, int.MaxValue)] public int RolID { get; set; }
        [Required] public string Direccion { get; set; } = "";
        [Required] public string Ciudad { get; set; } = "";
        [Required] public string Provincia { get; set; } = "";
        [Required] public string CodigoPostal { get; set; } = "";
    }
}
