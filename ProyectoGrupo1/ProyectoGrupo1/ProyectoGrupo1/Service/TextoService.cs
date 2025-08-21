
using Microsoft.Extensions.Configuration;

namespace ProyectoGrupo1.Services
{
    public class TextoService
    {
        private readonly IConfiguration _config;

        public TextoService(IConfiguration config)
        {
            _config = config;
        }

        public string ObtenerDescripcionContacto()
        {
            return _config["Textos:ContactoDescripcion"]
                   ?? "¿Tienes preguntas? Envíanos un mensaje y te responderemos lo antes posible.";
        }
    }
}
