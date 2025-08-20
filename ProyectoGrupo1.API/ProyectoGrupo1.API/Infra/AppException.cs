namespace ProyectoGrupo1.Api.Infra
{
    public class AppException : Exception
    {
        public int StatusCode { get; }
        public AppException(string message, int statusCode = 400, Exception? inner = null)
            : base(message, inner) => StatusCode = statusCode;
    }
}
