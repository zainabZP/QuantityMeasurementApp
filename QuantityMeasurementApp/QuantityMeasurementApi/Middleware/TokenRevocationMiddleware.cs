// using QuantityMeasurementApi.Services;
using QM.BusinessLogic.Service;
namespace QuantityMeasurementApi.Middleware;
// using QM.BusinessLogic.Interface;
using QM.Repository.Interface;  

public class TokenRevocationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenRevocationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklist)
    {
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader["Bearer ".Length..].Trim();
            if (blacklist.IsRevoked(token))
            {
                context.Response.StatusCode  = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { message = "Token has been revoked. Please login again." });
                return;
            }
        }
        await _next(context);
    }
}