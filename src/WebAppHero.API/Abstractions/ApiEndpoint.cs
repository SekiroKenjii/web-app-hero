using Microsoft.AspNetCore.Mvc;
using WebAppHero.Contract.Abstractions.Shared;

namespace WebAppHero.API.Abstractions;

public abstract class ApiEndpoint
{
    public const string BaseUrl = "/api/v{version:apiVersion}";

    private static ProblemDetails CreateProblemDetails(string title, int status, Error error, Error[]? errors = null)
    {
        return new() {
            Title = title,
            Status = status,
            Type = error.Code,
            Detail = error.Message,
            Extensions = { { nameof(errors), errors } }
        };
    }

    protected static IResult FailureHandler(Result result)
    {
        return result switch {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult => Results.BadRequest(CreateProblemDetails(
                title: "Validation Error",
                status: StatusCodes.Status400BadRequest,
                result.Error,
                validationResult.Errors
            )),
            _ => Results.BadRequest(CreateProblemDetails(
                title: "Bad Request",
                status: StatusCodes.Status400BadRequest,
                result.Error
            ))
        };
    }
}
