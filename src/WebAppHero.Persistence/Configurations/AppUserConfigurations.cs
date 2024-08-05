using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppHero.Domain.Entities.Identity;
using WebAppHero.Persistence.Constants;

namespace WebAppHero.Persistence.Configurations;

public sealed class AppUserConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable(TableNames.AppUsers);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.IsDirector).HasDefaultValue(false);
        builder.Property(x => x.IsHeadOfDepartment).HasDefaultValue(false);
        builder.Property(x => x.ManagerId).HasDefaultValue(null);
        builder.Property(x => x.IsRecipient).HasDefaultValue(-1);
        builder.Property(x => x.FirstName).HasMaxLength(255).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(255).IsRequired();

        builder.HasMany(x => x.Claims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
        builder.HasMany(x => x.Logins).WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
        builder.HasMany(x => x.Tokens).WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
        builder.HasMany(x => x.UserRoles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
    }
}
