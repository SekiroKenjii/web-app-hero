using Microsoft.AspNetCore.Identity;
using WebAppHero.Domain.Abstractions.Entities;

namespace WebAppHero.Domain.Entities.Identity;

public class AppUser : IdentityUser<Guid>, IAuditable
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public bool IsDirector { get; set; }

    public bool IsHeadOfDepartment { get; set; }

    public Guid? ManagerId { get; set; }

    public Guid PositionId { get; set; }

    public bool IsRecipient { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    // Relationships
    public virtual ICollection<IdentityUserClaim<Guid>>? Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>>? Logins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>>? Tokens { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>>? UserRoles { get; set; }
}
