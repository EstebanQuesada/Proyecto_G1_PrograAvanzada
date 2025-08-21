using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoGrupo1.Models
{
    public class ContactoMensaje
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Mensaje { get; set; }

        public DateTime FechaEnvio { get; set; } = DateTime.Now;
    }
}
