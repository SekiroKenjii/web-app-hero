using MediatR;

namespace WebAppHero.Contract.Abstractions.Messages;

public interface IDomainEventHandler : INotificationHandler<IDomainEvent>
{ }
