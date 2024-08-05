using Microsoft.AspNetCore.Identity;
using WebAppHero.Domain.Abstractions.Entities;

namespace WebAppHero.Domain.Entities.Identity;

public class AppRole : IdentityRole<Guid>, IAuditable
{
    public required string Description { get; set; }

    public required string RoleCode { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    // Relationships
    public virtual ICollection<IdentityUserRole<Guid>>? UserRoles { get; set; }
    public virtual ICollection<IdentityRoleClaim<Guid>>? Claims { get; set; }
}
