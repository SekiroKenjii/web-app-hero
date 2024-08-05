namespace WebAppHero.Domain.Abstractions.Entities;

public interface IDateTracking
{
    DateTimeOffset CreatedAt { get; set; }

    DateTimeOffset? ModifiedAt { get; set; }
}
