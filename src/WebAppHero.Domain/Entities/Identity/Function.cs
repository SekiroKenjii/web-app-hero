using WebAppHero.Domain.Abstractions;
using WebAppHero.Domain.Abstractions.Entities;
using WebAppHero.Domain.Enumerations;

namespace WebAppHero.Domain.Entities.Identity;

public class Function : EntityBase<int>, IDateTracking
{
    public required string Code { get; set; }

    public required string Name { get; set; }

    public required string Url { get; set; }

    public string? CssClass { get; set; }

    public int SortOrder { get; set; }

    public int ParentId { get; set; }

    public FunctionStatus Status { get; set; }

    public required string Key { get; set; }

    public int ActionValue { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }
}
