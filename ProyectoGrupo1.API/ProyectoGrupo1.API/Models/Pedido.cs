using System;
using System.Collections.Generic;

namespace ProyectoGrupo1.API.Models
{
    public class Pedido
    {
        public int PedidoID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime FechaPedido { get; set; }
        public int EstadoID { get; set; }
        public string? NombreEstado { get; set; }

        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }

    public class DetallePedido
    {
        public int DetallePedidoID { get; set; }
        public int PedidoID { get; set; }
        public int PTCID { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public string? ProductoNombre { get; set; }
        public int Stock { get; set; }
        public string? NombreColor { get; set; }
        public string? NombreTalla { get; set; }
        public string? UrlImagen { get; set; }
    }

    public class EstadoPedido
    {
        public int EstadoID { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
    }

    public class ProductoTallaColor
    {
        public int PTCID { get; set; }
        public int ProductoID { get; set; }
        public int TallaID { get; set; }
        public int ColorID { get; set; }
        public int Stock { get; set; }
        public string NombreCompuesto { get; set; } = string.Empty;
    }
}