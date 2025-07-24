using System;
using System.Collections.Generic;

namespace ProyectoGrupo1.Models
{
    public class Carrito
    {
        public int CarritoID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<DetalleCarrito> Detalles { get; set; } = new();
    }

    public class DetalleCarrito
    {
        public int DetalleID { get; set; }
        public int CarritoID { get; set; }
        public int PTCID { get; set; }
        public int Cantidad { get; set; }
        // Propiedades auxiliares para mostrar en la vista
        public string NombreProducto { get; set; }
        public string UrlImagen { get; set; }
        public string NombreTalla { get; set; }
        public string NombreColor { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
} 