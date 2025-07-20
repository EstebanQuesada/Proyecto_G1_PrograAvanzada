using System.ComponentModel.DataAnnotations;

namespace ProyectoGrupo1.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public string Ciudad { get; set; } = string.Empty;

        public string Provincia { get; set; } = string.Empty;

        public string CodigoPostal { get; set; } = string.Empty;

        public int RolID { get; set; }
    }
}
