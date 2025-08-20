namespace ProyectoGrupo1.Models
{
    public class ProductoTallaColor
    {
        public int PTCID { get; set; }
        public int ProductoID { get; set; }
        public int TallaID { get; set; }
        public int ColorID { get; set; }
        public int Stock { get; set; }
        public string NombreTalla { get; set; } = string.Empty;
        public string NombreColor { get; set; } = string.Empty;
        public string NombreCompuesto { get; set; } = string.Empty; 
    }
}
