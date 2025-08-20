using System.Text.Json;
using System.Text.Json.Serialization;
using ProyectoGrupo1.Models;
using Microsoft.AspNetCore.Http;

namespace ProyectoGrupo1.Service
{
    public class CarritoService
    {
        private readonly IHttpContextAccessor _http;
        private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        public CarritoService(IHttpContextAccessor http) => _http = http;

        private string Key(int usuarioId) => $"carrito:{usuarioId}";

        private List<CarritoDetalle> Read(int usuarioId)
        {
            var json = _http.HttpContext!.Session.GetString(Key(usuarioId));
            return string.IsNullOrWhiteSpace(json)
                ? new List<CarritoDetalle>()
                : (JsonSerializer.Deserialize<List<CarritoDetalle>>(json, _json) ?? new List<CarritoDetalle>());
        }

        private void Write(int usuarioId, List<CarritoDetalle> detalles)
        {
            var json = JsonSerializer.Serialize(detalles, _json);
            _http.HttpContext!.Session.SetString(Key(usuarioId), json);
        }

        private int NextDetalleId(List<CarritoDetalle> detalles) =>
            detalles.Count == 0 ? 1 : detalles.Max(d => d.DetalleID) + 1;

        public CarritoViewModel ObtenerCarritoPorUsuario(int usuarioId)
        {
            var detalles = Read(usuarioId);
            return new CarritoViewModel
            {
                UsuarioID = usuarioId,
                Detalles = detalles
            };
        }

        public void AgregarOActualizarProducto(int usuarioId, int ptcId, int cantidad, int? maxStock = null, CarritoItemMeta? meta = null)
        {
            if (cantidad < 1) cantidad = 1;
            if (maxStock.HasValue) cantidad = Math.Min(cantidad, Math.Max(1, maxStock.Value));

            var detalles = Read(usuarioId);
            var existente = detalles.FirstOrDefault(d => d.PTCID == ptcId);

            if (existente == null)
            {
                var nuevo = new CarritoDetalle
                {
                    DetalleID = NextDetalleId(detalles),
                    PTCID = ptcId,
                    Cantidad = cantidad
                };

                if (meta != null) AplicarMeta(nuevo, meta);
                detalles.Add(nuevo);
            }
            else
            {
                existente.Cantidad = cantidad;
                if (meta != null) AplicarMeta(existente, meta);
            }

            Write(usuarioId, detalles);
        }

        public void EliminarProducto(int usuarioId, int ptcId)
        {
            var detalles = Read(usuarioId);
            var idx = detalles.FindIndex(d => d.PTCID == ptcId);
            if (idx >= 0) { detalles.RemoveAt(idx); Write(usuarioId, detalles); }
        }

        public void VaciarCarrito(int usuarioId) => Write(usuarioId, new List<CarritoDetalle>());

        public int CantidadDe(int usuarioId, int ptcId)
        {
            var detalles = Read(usuarioId);
            return detalles.FirstOrDefault(d => d.PTCID == ptcId)?.Cantidad ?? 0;
        }

        private static void AplicarMeta(CarritoDetalle item, CarritoItemMeta meta)
        {
            if (meta.ProductoID.HasValue) item.ProductoID = meta.ProductoID.Value;
            if (!string.IsNullOrWhiteSpace(meta.NombreProducto)) item.NombreProducto = meta.NombreProducto!;
            if (!string.IsNullOrWhiteSpace(meta.Categoria)) item.Categoria = meta.Categoria!;
            if (!string.IsNullOrWhiteSpace(meta.Talla)) item.Talla = meta.Talla!;
            if (!string.IsNullOrWhiteSpace(meta.Color)) item.Color = meta.Color!;
            if (!string.IsNullOrWhiteSpace(meta.UrlImagen)) item.UrlImagen = meta.UrlImagen!;
            if (meta.PrecioUnitario.HasValue) item.PrecioUnitario = meta.PrecioUnitario.Value;
        }
    }

    public class CarritoViewModel
    {
        public int UsuarioID { get; set; }
        public List<CarritoDetalle> Detalles { get; set; } = new();

        [JsonIgnore]
        public int TotalItems => Detalles.Sum(d => d.Cantidad);

        [JsonIgnore]
        public decimal Total => Math.Round(Detalles.Sum(d => d.Subtotal), 2);
    }

    public class CarritoDetalle
    {
        public int DetalleID { get; set; }
        public int PTCID { get; set; }

        public int ProductoID { get; set; }                
        public string? NombreProducto { get; set; }        
        public string? Categoria { get; set; }             
        public string? Talla { get; set; }                 
        public string? Color { get; set; }                  
        public string? UrlImagen { get; set; }              
        public decimal PrecioUnitario { get; set; }        

        public int Cantidad { get; set; }

        [JsonIgnore]
        public decimal Subtotal => Math.Round(PrecioUnitario * Cantidad, 2);
    }

    public class CarritoItemMeta
    {
        public int? ProductoID { get; set; }
        public string? NombreProducto { get; set; }
        public string? Categoria { get; set; }
        public string? Talla { get; set; }
        public string? Color { get; set; }
        public string? UrlImagen { get; set; }
        public decimal? PrecioUnitario { get; set; }
    }
}
