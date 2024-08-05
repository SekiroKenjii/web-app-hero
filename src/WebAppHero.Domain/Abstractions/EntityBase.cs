using WebAppHero.Domain.Abstractions.Entities;

namespace WebAppHero.Domain.Abstractions;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public required TKey Id { get; set; }
}
