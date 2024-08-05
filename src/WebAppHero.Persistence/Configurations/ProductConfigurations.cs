using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppHero.Domain.Entities;
using WebAppHero.Persistence.Constants;

namespace WebAppHero.Persistence.Configurations;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Products);

        builder.HasKey(x => x.Id);
        //builder.HasIndex(x => x.IsDeleted).HasFilter("is_deleted = 0");

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).IsRequired(false);
        builder.Property(x => x.Price).HasDefaultValue(0).IsRequired();

        //builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
