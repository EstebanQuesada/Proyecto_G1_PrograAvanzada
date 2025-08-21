namespace ProyectoGrupo1.API.DTOs.ProductAdmin
{
    public class AdminProductoListItemDto
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public int CategoriaID { get; set; }
        public string Categoria { get; set; } = "";
        public string? UrlImagenPrincipal { get; set; }

        public bool Activo { get; set; }   
    }

    public class AdminProductoSaveDto
    {
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public decimal Precio { get; set; }
        public int CategoriaID { get; set; }
        public int MarcaID { get; set; }
        public int ProveedorID { get; set; }
        public List<string> Imagenes { get; set; } = new();
        public List<AdminPtcDto> PTCs { get; set; } = new();
    }

    public class AdminProductoDto : AdminProductoSaveDto
    {
        public int ProductoID { get; set; }

        public bool Activo { get; set; }   
    }

    public class AdminPtcDto
    {
        public int TallaID { get; set; }
        public int ColorID { get; set; }
        public int Stock { get; set; }
        public string? NombreTalla { get; set; }
        public string? NombreColor { get; set; }
        public int? PTCID { get; set; }
    }

    public class AdminLookupsDto
    {
        public List<LookupItem> Categorias { get; set; } = new();
        public List<LookupItem> Marcas { get; set; } = new();
        public List<LookupItem> Proveedores { get; set; } = new();
        public List<LookupItem> Tallas { get; set; } = new();
        public List<LookupItem> Colores { get; set; } = new();
    }

    public class LookupItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
    }
}
