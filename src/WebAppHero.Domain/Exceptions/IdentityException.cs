namespace WebAppHero.Domain.Exceptions;

public static class IdentityException
{
    public class UnauthorizedException(string message) : DomainException("Unauthorized", message) { }

    public class TokenException(string message) : DomainException("Token Exception", message) { }
}
