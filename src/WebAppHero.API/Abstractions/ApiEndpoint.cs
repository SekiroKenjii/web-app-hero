using Microsoft.AspNetCore.Mvc;
using WebAppHero.Contract.Abstractions.Shared;

namespace WebAppHero.API.Abstractions;

public abstract class ApiEndpoint
{
    public const string BaseUrl = "/api/v{version:apiVersion}";

    protected static IResult HandleResult<T>(RequestHandlerResult<T> requestHandlerResult) where T : Result
    {
        if (!requestHandlerResult.Result.IsSuccess)
        {
            return HandleFailureResult(requestHandlerResult);
        }

        return HandleSuccessResult(requestHandlerResult);
    }

    protected static IResult HandleSuccessResult<T>(RequestHandlerResult<T> requestHandlerResult) where T : Result
    {
        return requestHandlerResult.HttpStatusCode switch {
            StatusCodes.Status201Created => Results.Created(string.Empty, requestHandlerResult.Result),
            StatusCodes.Status204NoContent => Results.NoContent(),
            _ => Results.Ok(requestHandlerResult.Result)
        };
    }

    protected static IResult HandleFailureResult<T>(RequestHandlerResult<T> requestHandlerResult) where T : Result
    {
        if (requestHandlerResult is IValidationResult validationResult)
        {
            return Results.BadRequest(CreateProblemDetails(
                title: "Validation Error",
                status: StatusCodes.Status400BadRequest,
                requestHandlerResult.Result.Error,
                validationResult.Errors
            ));
        }

        return requestHandlerResult.HttpStatusCode switch {
            StatusCodes.Status403Forbidden => Results.Forbid(),
            StatusCodes.Status404NotFound => Results.NotFound(CreateProblemDetails(
                title: "Not Found",
                status: StatusCodes.Status404NotFound,
                requestHandlerResult.Result.Error
            )),
            StatusCodes.Status422UnprocessableEntity => Results.UnprocessableEntity(CreateProblemDetails(
                title: "Unprocessable Entity",
                status: StatusCodes.Status422UnprocessableEntity,
                requestHandlerResult.Result.Error
            )),
            _ => Results.BadRequest(CreateProblemDetails(
                title: "Bad Request",
                status: StatusCodes.Status400BadRequest,
                requestHandlerResult.Result.Error
            ))
        };
    }

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
}
