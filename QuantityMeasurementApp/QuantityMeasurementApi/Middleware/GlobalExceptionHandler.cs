using Microsoft.AspNetCore.Diagnostics;
using QM.Models.Exceptions;

namespace QuantityMeasurementApi.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var (statusCode, error) = exception switch
        {
            QuantityMeasurementException => (StatusCodes.Status400BadRequest, "Quantity Measurement Error"),
            ArgumentException            => (StatusCodes.Status400BadRequest, "Bad Request"),
            _                            => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        var errorResponse = new
        {
            timestamp = DateTime.UtcNow,
            status    = statusCode,
            error     = error,
            message   = exception.Message,
            path      = httpContext.Request.Path.Value
        };

        httpContext.Response.StatusCode  = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}