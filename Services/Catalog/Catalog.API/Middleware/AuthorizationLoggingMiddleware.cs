using Microsoft.AspNetCore.Http;

namespace Catalog.API.Middleware;

public class AuthorizationLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            Console.WriteLine($"Authorization: {authHeader}");
        }

        await _next(context);
    }
}
