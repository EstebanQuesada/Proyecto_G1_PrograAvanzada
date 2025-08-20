using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ProyectoGrupo1.API.DTOs.Product
{
    public class ProductoDetalleDto
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public decimal Precio { get; set; }
        public string Categoria { get; set; } = "";
        public List<string> Imagenes { get; set; } = new();
        public List<string> Tallas { get; set; } = new();
        public List<string> Colores { get; set; } = new();
        public List<ProductoTallaColorDto> PTCs { get; set; } = new();
    }
}
