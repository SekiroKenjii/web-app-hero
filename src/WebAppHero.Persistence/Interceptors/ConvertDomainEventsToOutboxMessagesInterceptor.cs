using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using WebAppHero.Domain.Abstractions.Aggregates;
using WebAppHero.Persistence.Outbox;

namespace WebAppHero.Persistence.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var outboxMessages = dbContext.ChangeTracker
            .Entries<AggregateRoot<Guid>>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot => {
                var domainEvents = aggregateRoot.DomainEvents();

                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage {
                Id = Guid.NewGuid(),
                OccurredAt = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    value: domainEvent,
                    settings: new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }
                )
            })
            .ToList();

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
