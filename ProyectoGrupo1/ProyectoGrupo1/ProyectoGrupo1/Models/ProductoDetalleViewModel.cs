namespace ProyectoGrupo1.Models
{
    using System.Collections.Generic;

    public class ProductoDetalleViewModel
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }
        public List<string> Imagenes { get; set; }
        public List<string> Tallas { get; set; }
        public List<string> Colores { get; set; }
        public List<ProductoTallaColor> PTCs { get; set; } = new();

    }

}
