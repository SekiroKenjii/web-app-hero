using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAppHero.Domain.Abstractions.Entities;
using WebAppHero.Domain.Entities;
using WebAppHero.Domain.Entities.Identity;

namespace WebAppHero.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<AppUser, AppRole, Guid>(options)
{
    private static LambdaExpression? GenerateQueryFilterLambda(
        Type type,
        object expressionConstant,
        string propertyOrFieldName,
        bool isEqual = true)
    {
        var parameter = Expression.Parameter(type, "x");
        var expressionConstantValue = Expression.Constant(expressionConstant);
        var property = Expression.PropertyOrField(parameter, propertyOrFieldName);
        var eqExpression = isEqual
            ? Expression.Equal(property, expressionConstantValue)
            : Expression.NotEqual(property, expressionConstantValue);

        return Expression.Lambda(eqExpression, parameter);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var softDeleteEntities = typeof(ISoftDelete).Assembly.GetTypes()
            .Where(x => typeof(ISoftDelete).IsAssignableFrom(x) && x.IsClass);

        foreach (var softDeleteEntity in softDeleteEntities)
        {
            builder.Entity(softDeleteEntity)
                .HasQueryFilter(GenerateQueryFilterLambda(
                    type: softDeleteEntity,
                    expressionConstant: false,
                    propertyOrFieldName: nameof(ISoftDelete.IsDeleted)
                ));

            builder.Entity(softDeleteEntity).HasIndex(nameof(ISoftDelete.IsDeleted)).HasFilter("is_deleted = 0");
        }

        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<AppRole> AppRoles { get; set; }

    public DbSet<Function> Functions { get; set; }

    public DbSet<Product> Products { get; set; }
}
