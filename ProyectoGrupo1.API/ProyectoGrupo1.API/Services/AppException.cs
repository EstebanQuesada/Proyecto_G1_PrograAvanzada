namespace ProyectoGrupo1.Api.Services;

public class AppException : Exception
{
    public int StatusCode { get; }

    public AppException(string message, int statusCode, Exception? inner = null)
        : base(message, inner) => StatusCode = statusCode;
}
