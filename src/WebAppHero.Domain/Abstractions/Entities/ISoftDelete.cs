namespace WebAppHero.Domain.Abstractions.Entities;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }

    DateTimeOffset? DeletedAt { get; set; }

    void Undo()
    {
        IsDeleted = false;
        DeletedAt = null;
    }
}
