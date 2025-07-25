﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoGrupo1.Models
{
    public class Pedido
    {
        public int PedidoID { get; set; }
        public DateTime FechaPedido { get; set; }
        public string NombreEstado { get; set; } = string.Empty;

        public List<DetallePedido> Detalles { get; set; } = new();
    }

    public class DetallePedido
    {
        public int DetallePedidoID { get; set; }
        public int PTCID { get; set; } // Necesario para volver a pedir
        public string Producto { get; set; } = string.Empty;
        public string NombreColor { get; set; } = string.Empty;
        public string NombreTalla { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string UrlImagen { get; set; } = string.Empty;
    }

    public class HistorialPedidoViewModel
    {
        public List<Pedido> Pedidos { get; set; } = new();

        public List<EstadoPedido> Estados { get; set; } = new();

        public List<ProductoTallaColor> ProductosPTC { get; set; } = new();

        public NuevoPedidoInputModel NuevoPedido { get; set; } = new();
    }

    public class NuevoPedidoInputModel
    {
        [Required(ErrorMessage = "Debe seleccionar un estado.")]
        public int EstadoID { get; set; }

        [MinLength(1, ErrorMessage = "Agrega al menos un detalle.")]
        public List<NuevoDetallePedidoInputModel> Detalles { get; set; } = new();
    }

    public class NuevoDetallePedidoInputModel
    {
        [Required(ErrorMessage = "Debe seleccionar un producto.")]
        public int PTCID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal PrecioUnitario { get; set; }
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
        public string NombreTalla { get; set; } = string.Empty;
        public string NombreColor { get; set; } = string.Empty;
    }
}