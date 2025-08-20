using Microsoft.AspNetCore.Mvc;

namespace ProyectoGrupo1.API.DTOs.Product
{
    public class ProductoCatalogoDto
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; } = "";
        public string DescripcionCorta { get; set; } = "";
        public decimal Precio { get; set; }
        public string Categoria { get; set; } = "";
        public string UrlImagenPrincipal { get; set; } = "";
    }
}
