using WebAppHero.Domain.Abstractions.Aggregates;
using WebAppHero.Domain.Abstractions.Entities;

namespace WebAppHero.Domain.Entities;

public class Product : AggregateRoot<Guid>, IEntityAuditBase<Guid>
{
    public required string Name { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public static Product Create(Guid id, string name, decimal price, string? description)
    {
        var product = new Product {
            Id = id,
            Name = name,
            Price = price,
            Description = description
        };

        product.RaiseDomainEvent(new Contract.Services.V1.Product.DomainEvent.ProductCreated(
            product.NewEventId(), product.Id, product.Name, product.Price, product.Description)
        );

        return product;
    }

    public void Update(string name, decimal price, string? description)
    {
        Name = name;
        Price = price;
        Description = description;

        RaiseDomainEvent(new Contract.Services.V1.Product.DomainEvent.ProductUpdated(NewEventId(), Id, name, price, description));
    }

    public void Delete()
    {
        RaiseDomainEvent(new Contract.Services.V1.Product.DomainEvent.ProductDeleted(NewEventId(), Id));
    }
}
