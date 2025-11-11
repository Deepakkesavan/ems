using System.Diagnostics;
using System.Net;
using System.Text.Json;
using GlobalExceptionHandler.Exceptions;
using GlobalExceptionHandler.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace GlobalExceptionHandler
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex,traceId);

            }
        }
        public  async Task HandleExceptionAsync(HttpContext context, Exception exception,string traceId)
        {
            ErrorResponse errorResponse = new();
            string result;
            switch (exception)
            {
                case InvalidOperationException:
                    errorResponse.StatusCode = HttpStatusCode.BadRequest;
                    errorResponse.StackTrace = exception.StackTrace;
                    errorResponse.TraceId = traceId;
                    errorResponse.Message = exception.Message+exception.InnerException;
                    break;
                case DataNotFoundException:
                    errorResponse.StatusCode = HttpStatusCode.NotFound;
                    errorResponse.StackTrace = exception.StackTrace;
                    errorResponse.TraceId = traceId;
                    errorResponse.Message = exception.Message + exception.InnerException ?? "Data not found";
                    break;
                case CacheNotFoundException:
                    errorResponse.StatusCode = HttpStatusCode.NotFound;
                    errorResponse.StackTrace = exception.StackTrace;
                    errorResponse.TraceId = traceId;
                    errorResponse.Message = exception.Message + exception.InnerException ?? "Cache not found";
                    break;
                default:
                    errorResponse.StatusCode = HttpStatusCode.InternalServerError;
                    errorResponse.StackTrace = exception.StackTrace;
                    errorResponse.TraceId = traceId;
                    errorResponse.Message = exception.Message + exception.InnerException ?? "Internal Server Error";
                    break;
            }

                //result = JsonSerializer.Serialize(new
                //{
                //    error = errorResponse.Message,
                //    //inner= errorResponse.Inner
                //    traceId = errorResponse.TraceId,
                //    status = (int)errorResponse.StatusCode,
                //    stack = errorResponse.StackTrace
                //}, new JsonSerializerOptions { WriteIndented = true });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)errorResponse.StatusCode;

            
            var baseResponse = new BaseResponse<object>
            {
                Errors = new List<ErrorResponse> { errorResponse },
                Result = null,
                Warnings = null
            };
            var json = JsonSerializer.Serialize(baseResponse);
            await context.Response.WriteAsync(json);
            //return baseResponse;
        }
    }
}

