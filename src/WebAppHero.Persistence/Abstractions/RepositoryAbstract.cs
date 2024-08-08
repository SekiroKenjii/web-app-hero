using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAppHero.Domain.Abstractions;

namespace WebAppHero.Persistence.Abstractions;

/// <summary>
/// Provides the most basic repository implementation for data access operations.
/// </summary>
/// <typeparam name="TContext">The type of the database context.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the entity key.</typeparam>
public abstract class RepositoryAbstract<TContext, TEntity, TKey>
    where TContext : DbContext
    where TEntity : EntityBase<TKey>
{
    /// <summary>
    /// Retrieves entities from the context with optional tracking changes and include expressions.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="isTracking">Specifies whether the entities should be tracked by the context.</param>
    /// <param name="includes">The related entities to include in the query.</param>
    /// <returns>An <see cref="IQueryable{TEntity}"/> representing the query.</returns>
    public static IQueryable<TEntity> GetEntity(
        TContext dbContext,
        bool isTracking,
        params Expression<Func<TEntity, object>>[]? includes)
    {
        var entity = isTracking
            ? dbContext.Set<TEntity>().AsTracking()
            : dbContext.Set<TEntity>().AsNoTracking();

        if (includes != null && includes.Length != 0)
        {
            foreach (var item in includes)
            {
                entity.Include(item);
            }
        }

        return entity.AsSplitQuery();
    }

    /// <summary>
    /// A compiled query that asynchronously retrieves a single entity that matches the specified predicate.
    /// </summary>
    public static readonly Func<TContext, Expression<Func<TEntity, bool>>, Task<TEntity?>> SingleOrDefaultAsync =
        EF.CompileAsyncQuery((TContext dbContext, Expression<Func<TEntity, bool>> pred) =>
            GetEntity(dbContext, true).Where(pred).SingleOrDefault());


    /// <summary>
    /// A compiled query that asynchronously retrieves the first entity that matches the specified key.
    /// </summary>
    public static readonly Func<TContext, TKey, Task<TEntity?>> FirstOrDefaultByIdAsync =
        EF.CompileAsyncQuery((TContext dbContext, TKey id) =>
            GetEntity(dbContext, true).Where(x => !Equals(x.Id, null) && x.Id.Equals(id)).FirstOrDefault());
}
