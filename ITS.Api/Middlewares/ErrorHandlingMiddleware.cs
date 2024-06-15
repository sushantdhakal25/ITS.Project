using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Net;

namespace ITS.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError($"Exception Message: {exception.Message}");
            _logger.LogError($"Exception StackTrace: {exception.StackTrace}");
            _logger.LogError($"Exception InnerException: {exception.InnerException}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                MySqlException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var errorMessage = exception is MySqlException ? "Error occoured in database" : exception.Message;
            var result = JsonConvert.SerializeObject(new
            {   error = errorMessage
            });

            return context.Response.WriteAsync(result);
        }
    }
}
