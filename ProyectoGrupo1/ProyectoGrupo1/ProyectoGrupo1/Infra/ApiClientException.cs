
using System.Net;

public class ApiClientException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string? RawBody { get; }

    public ApiClientException(string message, HttpStatusCode statusCode, string? rawBody = null)
        : base(message) { StatusCode = statusCode; RawBody = rawBody; }
}
