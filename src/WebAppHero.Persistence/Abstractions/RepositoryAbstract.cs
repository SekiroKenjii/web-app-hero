using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAppHero.Domain.Abstractions;

namespace WebAppHero.Persistence.Abstractions;

public abstract class RepositoryAbstract<TContext, TEntity, TKey>
    where TContext : DbContext
    where TEntity : EntityBase<TKey>
{
    public static IQueryable<TEntity> GetEntity(TContext dbContext, params Expression<Func<TEntity, object>>[]? includes)
    {
        var entity = dbContext.Set<TEntity>().AsNoTracking();

        if (includes != null && includes.Length != 0)
        {
            foreach (var item in includes)
            {
                entity.Include(item);
            }
        }

        return entity.AsSplitQuery();
    }

    public static readonly Func<TContext, Expression<Func<TEntity, bool>>?, Expression<Func<TEntity, object>>[]?, Task<TEntity?>> SingleOrDefaultAsync =
        EF.CompileAsyncQuery((TContext dbContext, Expression<Func<TEntity, bool>>? predicate, Expression<Func<TEntity, object>>[]? includes) =>
            predicate != null
                ? GetEntity(dbContext, includes).SingleOrDefault(predicate)
                : GetEntity(dbContext, includes).SingleOrDefault());

    public static readonly Func<TContext, Expression<Func<TEntity, bool>>?, Expression<Func<TEntity, object>>[]?, Task<TEntity?>> FirstOrDefaultAsync =
        EF.CompileAsyncQuery((TContext dbContext, Expression<Func<TEntity, bool>>? predicate, Expression<Func<TEntity, object>>[]? includes) =>
            predicate != null
                ? GetEntity(dbContext, includes).FirstOrDefault(predicate)
                : GetEntity(dbContext, includes).FirstOrDefault());
}
