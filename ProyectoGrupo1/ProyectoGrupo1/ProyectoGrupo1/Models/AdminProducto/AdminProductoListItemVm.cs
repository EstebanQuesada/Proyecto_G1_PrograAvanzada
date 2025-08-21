namespace ProyectoGrupo1.Models.AdminProducto;

public class AdminProductoListItemVm
{
    public int ProductoID { get; set; }
    public string Nombre { get; set; } = "";
    public decimal Precio { get; set; }
    public string Categoria { get; set; } = "";
    public string? UrlImagenPrincipal { get; set; }
}
