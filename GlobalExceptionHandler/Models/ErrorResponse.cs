using System.Net;

namespace GlobalExceptionHandler.Models
{
    public class ErrorResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? TraceId { get; set; }
    }
}
