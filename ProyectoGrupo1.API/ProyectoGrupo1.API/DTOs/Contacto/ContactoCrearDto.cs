
using System.ComponentModel.DataAnnotations;

namespace ProyectoGrupo1.API.DTOs.Contacto
{
    public class ContactoCrearDto
    {
        [Required, StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required, EmailAddress, StringLength(150)]
        public string Correo { get; set; } = null!;

        [Required, StringLength(1000)]
        public string Mensaje { get; set; } = null!;
    }
}

