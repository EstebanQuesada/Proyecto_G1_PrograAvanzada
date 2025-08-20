using System;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoGrupo1.Models
{
    public class HistorialPedidoViewModel
    {
        public List<PedidoHistorialVm> Pedidos { get; set; } = new();
    }

    public class PedidoHistorialVm
    {
        public int PedidoID { get; set; }
        public DateTime FechaPedido { get; set; }
        public string NombreEstado { get; set; } = "";
        public List<PedidoHistorialDetalleVm> Detalles { get; set; } = new();

        public decimal Total => Detalles.Sum(d => d.PrecioUnitario * d.Cantidad);
    }

    public class PedidoHistorialDetalleVm
    {
        public int DetallePedidoID { get; set; }
        public int PTCID { get; set; }
        public string Producto { get; set; } = "";
        public string NombreTalla { get; set; } = "";
        public string NombreColor { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string? UrlImagen { get; set; }
    }
}
