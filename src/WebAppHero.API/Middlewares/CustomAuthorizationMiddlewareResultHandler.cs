using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Newtonsoft.Json;

namespace WebAppHero.API.Middlewares;

public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        if (authorizeResult.Challenged)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                title = "Authentication Error",
                status = context.Response.StatusCode,
                detail = "You are not authorized!",
                errors = Enumerable.Empty<object>(),
            }));

            return;
        }

        if (authorizeResult.Forbidden)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                title = "Authentication Error",
                status = context.Response.StatusCode,
                detail = "You are not authorized to access this resource!",
                errors = Enumerable.Empty<object>(),
            }));

            return;
        }

        // Fall back to the default implementation.
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
