namespace ProyectoGrupo1.API.DTOs.Pedido
{
    public class HistorialPedidoDto
    {
        public int PedidoID { get; set; }
        public DateTime FechaPedido { get; set; }
        public string NombreEstado { get; set; } = "";
        public int DetallePedidoID { get; set; }
        public int PTCID { get; set; }
        public string Producto { get; set; } = "";
        public int Stock { get; set; }
        public string NombreColor { get; set; } = "";
        public string NombreTalla { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string? UrlImagen { get; set; }
    }
}
