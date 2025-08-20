using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using ProyectoGrupo1.Models;

namespace ProyectoGrupo1.Service
{
    public class CarritoService
    {
        private readonly IHttpContextAccessor _http;
        private const string SessionKeyPrefix = "CARRITO_";

        private static readonly JsonSerializerOptions _jsonOpts = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        public CarritoService(IHttpContextAccessor http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }

        private ISession Session =>
            _http.HttpContext?.Session
            ?? throw new InvalidOperationException("No hay HttpContext/Session disponible.");

        private string Key(int usuarioId) => SessionKeyPrefix + usuarioId;


        public Carrito ObtenerCarritoPorUsuario(int usuarioId)
        {
            var key = Key(usuarioId);
            var json = Session.GetString(key);
            if (string.IsNullOrWhiteSpace(json))
                return new Carrito { UsuarioID = usuarioId };

            try
            {
                var carrito = JsonSerializer.Deserialize<Carrito>(json, _jsonOpts)
                              ?? new Carrito { UsuarioID = usuarioId };
                carrito.UsuarioID = usuarioId; 
                return carrito;
            }
            catch
            {
                return new Carrito { UsuarioID = usuarioId };
            }
        }

        public void GuardarCarrito(int usuarioId, Carrito carrito)
        {
            carrito.UsuarioID = usuarioId;
            var json = JsonSerializer.Serialize(carrito, _jsonOpts);
            Session.SetString(Key(usuarioId), json);
        }

        public void VaciarCarrito(int usuarioId)
        {
            Session.Remove(Key(usuarioId));
        }

        public void EliminarProducto(int usuarioId, int ptcId)
        {
            var carrito = ObtenerCarritoPorUsuario(usuarioId);
            var det = carrito.Detalles.FirstOrDefault(d => d.PTCID == ptcId);
            if (det != null)
            {
                carrito.Detalles.Remove(det);
                GuardarCarrito(usuarioId, carrito);
            }
        }

        public void AgregarOActualizarProducto(
            int usuarioId,
            int ptcId,
            int cantidad,
            int maxStock,
            string? nombreProducto = null,
            string? nombreTalla = null,
            string? nombreColor = null,
            decimal? precioUnitario = null,
            string? urlImagen = null)
        {
            var carrito = ObtenerCarritoPorUsuario(usuarioId);

            if (cantidad < 1) cantidad = 1;
            var cantFinal = Math.Min(cantidad, maxStock);

            var det = carrito.Detalles.FirstOrDefault(d => d.PTCID == ptcId);
            if (det == null)
            {
                det = new DetalleCarrito
                {
                    DetalleID = GenerarDetalleId(carrito),
                    PTCID = ptcId,
                    Cantidad = cantFinal,
                    NombreProducto = nombreProducto ?? "",
                    NombreTalla = nombreTalla ?? "",
                    NombreColor = nombreColor ?? "",
                    PrecioUnitario = precioUnitario ?? 0,
                    UrlImagen = urlImagen ?? ""
                };
                carrito.Detalles.Add(det);
            }
            else
            {
                det.Cantidad = cantFinal;
                if (!string.IsNullOrWhiteSpace(nombreProducto)) det.NombreProducto = nombreProducto;
                if (!string.IsNullOrWhiteSpace(nombreTalla)) det.NombreTalla = nombreTalla;
                if (!string.IsNullOrWhiteSpace(nombreColor)) det.NombreColor = nombreColor;
                if (precioUnitario.HasValue) det.PrecioUnitario = precioUnitario.Value;
                if (!string.IsNullOrWhiteSpace(urlImagen)) det.UrlImagen = urlImagen;
            }

            GuardarCarrito(usuarioId, carrito);
        }


        private static int GenerarDetalleId(Carrito carrito)
        {
            if (carrito.Detalles.Count == 0) return 1;
            return carrito.Detalles.Max(d => d.DetalleID) + 1;
        }
    }
}
