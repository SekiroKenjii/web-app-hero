using WebAppHero.Contract.Abstractions.Messages;

namespace WebAppHero.Contract.Services.V1.Product;

public static class DomainEvent
{
    public record ProductCreated(Guid EventId, Guid Id, string Name, decimal Price, string? Description) : IDomainEvent, ICommand;

    public record ProductUpdated(Guid EventId, Guid Id, string Name, decimal Price, string? Description) : IDomainEvent, ICommand;

    public record ProductDeleted(Guid EventId, Guid Id) : IDomainEvent, ICommand;
}
