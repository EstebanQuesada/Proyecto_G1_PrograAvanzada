using Microsoft.AspNetCore.Mvc;

namespace ProyectoGrupo1.API.DTOs.Product
{
    public class ProductoTallaColorDto
    {
        public int PTCID { get; set; }
        public int ProductoID { get; set; }
        public int TallaID { get; set; }
        public int ColorID { get; set; }
        public int Stock { get; set; }
        public string NombreTalla { get; set; } = "";
        public string NombreColor { get; set; } = "";
    }
}
