namespace ProyectoGrupo1.Models.AdminProducto
{
    public sealed class AdminProductoListItemVm
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public int CategoriaID { get; set; }
        public string Categoria { get; set; } = "";
        public string? UrlImagenPrincipal { get; set; }

        public bool Activo { get; set; }   
    }
}

