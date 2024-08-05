using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppHero.Domain.Entities.Identity;
using WebAppHero.Persistence.Constants;

namespace WebAppHero.Persistence.Configurations;

public sealed class AppRoleConfigurations : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable(TableNames.AppRoles);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).HasMaxLength(250).IsRequired();
        builder.Property(x => x.RoleCode).HasMaxLength(50).IsRequired();

        builder.HasMany(x => x.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        builder.HasMany(x => x.UserRoles).WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
    }
}
