namespace WebAppHero.Domain.Abstractions.Entities;

public interface IEntityAuditBase<TKey> : IEntityBase<TKey>, IAuditable { }