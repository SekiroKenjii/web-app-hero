using WebAppHero.Domain.Abstractions;

namespace WebAppHero.Domain.Entities;

public class Product : EntityAuditBase<Guid>
{
    public required string Name { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }
}
