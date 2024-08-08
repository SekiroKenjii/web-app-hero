namespace WebAppHero.Domain.Exceptions;

public class NotFoundException(string message) : DomainException("Not Found", message)
{ }
