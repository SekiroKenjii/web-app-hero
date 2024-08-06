using WebAppHero.Contract.Abstractions.Messages;

namespace WebAppHero.Domain.Abstractions.Aggregates;

public abstract class AggregateRoot<TKey> : EntityBase<TKey>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents() => [.. _domainEvents];

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
