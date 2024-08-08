using System.Text.Json;
using WebAppHero.Domain.Exceptions;

namespace WebAppHero.API.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error has occurred: {Message}", e.Message);

            if (httpContext.Response.HasStarted)
            {
                return;
            }

            await HandleExceptionAsync(httpContext, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var response = new {
            title = GetExceptionTitle(exception),
            status = statusCode,
            detail = statusCode >= 500 ? "An unhandled error has occurred." : exception.Message,
            errors = GetValidationErrors(exception),
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception)
    {
        return exception switch {
            IdentityException.TokenException or IdentityException.UnauthorizedException => StatusCodes.Status401Unauthorized,
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            FormatException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string GetExceptionTitle(Exception exception)
    {
        return exception switch {
            DomainException domainException => domainException.Title,
            _ => "Internal Server Error"
        };
    }

    private static IReadOnlyCollection<Application.Exceptions.ValidationError>? GetValidationErrors(Exception exception)
    {
        if (exception is Application.Exceptions.ValidationException validationException)
        {
            return validationException.Errors;
        }

        return default;
    }
}
