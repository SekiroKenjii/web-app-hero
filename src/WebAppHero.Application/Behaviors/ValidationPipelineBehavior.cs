using FluentValidation;
using MediatR;
using WebAppHero.Contract.Abstractions.Shared;

namespace WebAppHero.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : Result
{
    private static T? CreateValidationResult<T>(Error[] errors) where T : Result
    {
        if (typeof(T) == typeof(Result))
        {
            return ValidationResult.WithErrors(errors) as T;
        }

        var result = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(T).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))?
            .Invoke(null, [errors]);

        return (T?)result;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var errors = validators
            .Select(x => x.Validate(request))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .Select(x => new Error(x.PropertyName, x.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Length != 0)
        {
            return CreateValidationResult<TResponse>(errors)
                ?? throw new InvalidOperationException("An error occurred while attempting to generate the validation result.");
        }

        return await next();
    }
}
