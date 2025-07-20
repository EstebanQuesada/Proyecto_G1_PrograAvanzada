namespace ProyectoGrupo1.Models
{
    public class DireccionUsuario
    {
        public int DireccionID { get; set; }
        public int UsuarioID { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public DireccionUsuario DireccionEnvio { get; set; }

    }
}
