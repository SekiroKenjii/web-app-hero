using WebAppHero.Domain.Exceptions;

namespace WebAppHero.Application.Exceptions;

public class ValidationException : DomainException
{
    protected ValidationException(IReadOnlyCollection<ValidationError> errors)
        : base("Validation Failure", "One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public IReadOnlyCollection<ValidationError> Errors { get; set; }
}

public record ValidationError(string PropertyName, string ErrorMessage);
