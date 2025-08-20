namespace ProyectoGrupo1.API.DTOs.Pedido
{
    public class PedidoCrearDto
    {
        public int UsuarioID { get; set; }
        public List<PedidoDetalleCrearDto> Detalles { get; set; } = new();
    }

    public class PedidoDetalleCrearDto
    {
        public int PTCID { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
