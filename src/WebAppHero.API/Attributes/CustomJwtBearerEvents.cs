using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAppHero.Application.Abstractions;
using WebAppHero.Contract.Services.V1.Identity;

namespace WebAppHero.API.Attributes;

public class CustomJwtBearerEvents(ICacheService cacheService) : JwtBearerEvents
{
    public override async Task TokenValidated(TokenValidatedContext context)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        if (context.SecurityToken is JwtSecurityToken accessToken)
        {
            var requestToken = accessToken.RawData.ToString();

            var emailKey = accessToken.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers.Append("IS-TOKEN-INVALID", "true");
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                    title = "Authentication Error",
                    status = context.Response.StatusCode,
                    detail = "Invalid access token!",
                    errors = Enumerable.Empty<object>(),
                }));

                return;
            }

            var authenticated = await cacheService.GetAsync<Response.Authenticated>(emailKey);

            if (authenticated is null || authenticated.AccessToken != requestToken)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers.Append("IS-TOKEN-REVOKED", "true");
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                    title = "Authentication Error",
                    status = context.Response.StatusCode,
                    detail = "Token has been revoked!",
                    errors = Enumerable.Empty<object>(),
                }));
            }

            return;
        }

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonConvert.SerializeObject(new {
            title = "Authentication Error",
            status = context.Response.StatusCode,
            detail = "Invalid access token schema!",
            errors = Enumerable.Empty<object>(),
        }));
    }
}
