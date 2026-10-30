using System;

namespace LearnWebAPI.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException(string message, int statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using LearnWebAPI.Exceptions;

namespace LearnWebAPI.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = ex is ApiException apiException
                    ? apiException.StatusCode
                    : StatusCodes.Status500InternalServerError;

                var traceId = context.TraceIdentifier;

                _logger.LogError(
                    ex,
                    "Error occurred while processing request. TraceId: {TraceId}",
                    traceId);

                context.Response.Clear();
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";

                var problemDetails = new ProblemDetails
                {
                    Type = "about:blank",
                    Title = GetErrorTitle(statusCode),
                    Status = statusCode,
                    Instance = context.Request.Path,
                    Detail = statusCode == 500
                        ? $"An internal server error occurred. TraceId: {traceId}"
                        : ex.Message
                };

                problemDetails.Extensions["traceId"] = traceId;

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }

        private static string GetErrorTitle(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                409 => "Conflict",
                422 => "Unprocessable Entity",
                500 => "Internal Server Error",
                429 => "Too Many Requests",
                _ => "Request Failed"
            };
        }
    }
}