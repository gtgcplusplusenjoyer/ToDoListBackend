using Microsoft.EntityFrameworkCore;
using ToDoList.Infrastructure.Exceptions;

namespace ToDoList.Infrastructure.MiddleWares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

            var (statusCode, userMessage) = MapExceptionToResponse(exception);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorResponse = new
            {
                statusCode = statusCode,
                message = userMessage,
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }

        private static (int statusCode, string message) MapExceptionToResponse(Exception exception)
        {
            return exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Requested resource was not found"),

                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Authentication required"),

                DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "Data was modified by another user"),

                DbUpdateException => (StatusCodes.Status400BadRequest, "Database operation failed"),

                ArgumentException or ArgumentNullException => (StatusCodes.Status400BadRequest, "Invalid request parameters"),

                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };
        }

    }
}
