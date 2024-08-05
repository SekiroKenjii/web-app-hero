using MediatR;

namespace WebAppHero.Contract.Abstractions.Messages;

public interface IDomainEvent : INotification
{
    public Guid EventId { get; }
}
