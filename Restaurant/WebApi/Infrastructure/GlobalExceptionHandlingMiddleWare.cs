using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.Infrastructure
{
    public class GlobalExceptionHandlingMiddleWare : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleWare> _logger;
        public GlobalExceptionHandlingMiddleWare(ILogger<GlobalExceptionHandlingMiddleWare> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception.Message);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            Microsoft.AspNetCore.Mvc.ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "Server Error",
                Title = "Server Error",
                Detail = "Internal Server error has occurred"
            };

            var jsonDetails = JsonConvert.SerializeObject(problemDetails);
            await httpContext.Response.WriteAsJsonAsync(jsonDetails, cancellationToken);

            return true;
        }
    }
}
