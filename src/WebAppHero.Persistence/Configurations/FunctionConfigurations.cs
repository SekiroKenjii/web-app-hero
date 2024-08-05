using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppHero.Domain.Entities.Identity;
using WebAppHero.Domain.Enumerations;
using WebAppHero.Persistence.Constants;

namespace WebAppHero.Persistence.Configurations;

public sealed class FunctionConfigurations : IEntityTypeConfiguration<Function>
{
    public void Configure(EntityTypeBuilder<Function> builder)
    {
        builder.ToTable(TableNames.Functions);

        builder.Property(t => t.Id).HasDefaultValue(0);
        builder.Property(t => t.Code).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Key).HasMaxLength(50).IsRequired();
        builder.Property(t => t.ActionValue).IsRequired();
        builder.Property(t => t.ParentId).HasDefaultValue(-1);
        builder.Property(t => t.CssClass).HasMaxLength(10).HasDefaultValue(null);
        builder.Property(t => t.Url).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Status).HasDefaultValue(FunctionStatus.Active);
        builder.Property(t => t.SortOrder).HasDefaultValue(-1);
        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.ModifiedAt).IsRequired();
    }
}
