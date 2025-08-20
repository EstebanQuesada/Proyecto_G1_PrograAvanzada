using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoGrupo1.API.Models
{
    public class NuevoPedidoInputModel
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int EstadoID { get; set; }

        [Required]
        public List<NuevoDetallePedidoInputModel> Detalles { get; set; } = new List<NuevoDetallePedidoInputModel>();
    }

    public class NuevoDetallePedidoInputModel
    {
        [Required]
        public int PTCID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a cero")]
        public decimal PrecioUnitario { get; set; }
    }
}