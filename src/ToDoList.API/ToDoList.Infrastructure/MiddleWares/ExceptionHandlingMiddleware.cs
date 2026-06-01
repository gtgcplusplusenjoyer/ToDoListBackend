using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using ToDoList.Infrastructure.Dto;
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

            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.NotFound, "Not found");
            }

            catch (DbUpdateException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest, "Error with update database");
            }

            catch (ArgumentNullException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest, "Error. Method does not accept null as a valid argument");
            }

            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest, "Invalid operation exception");
            }

            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex.Message, HttpStatusCode.InternalServerError, "An unexpected error occurred");
            }
        }

        public async Task HandleExceptionAsync(HttpContext httpContext, string exMsg, HttpStatusCode httpStatusCode, string message)
        {
            _logger.LogError(exMsg);

            HttpResponse response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            ErrorDto errorDto = new()
            {
                Message = message,
                StatusCode = (int)httpStatusCode
            };

            await response.WriteAsJsonAsync(errorDto);
        }

    }
}
