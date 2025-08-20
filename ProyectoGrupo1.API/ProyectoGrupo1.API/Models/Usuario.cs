namespace ProyectoGrupo1.Api.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Correo { get; set; } = "";
        public string Contrasena { get; set; } = ""; 
        public string Direccion { get; set; } = "";
        public string Ciudad { get; set; } = "";
        public string Provincia { get; set; } = "";
        public string CodigoPostal { get; set; } = "";
        public int RolID { get; set; }
    }
}
