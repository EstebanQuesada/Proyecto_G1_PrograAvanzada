namespace ProyectoGrupo1.API.Models
{
    public class ContactoMensaje
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public DateTime FechaEnvio { get; set; }
        public bool Visto { get; set; }
        public bool Completado { get; set; }
        public DateTime? FechaVisto { get; set; }
        public DateTime? FechaCompletado { get; set; }
    }
}