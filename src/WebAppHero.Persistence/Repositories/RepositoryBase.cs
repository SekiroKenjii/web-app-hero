using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAppHero.Domain.Abstractions;
using WebAppHero.Domain.Abstractions.Repositories;

namespace WebAppHero.Persistence.Repositories;

public sealed class RepositoryBase<TEntity, TKey>(DbContext? dbContext) : IRepositoryBase<TEntity, TKey>, IDisposable
    where TEntity : EntityBase<TKey>
{
    private readonly DbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    private IQueryable<TEntity> GetEntity(params Expression<Func<TEntity, object>>[] includes)
    {
        var entity = _dbContext.Set<TEntity>().AsNoTracking();

        if (includes != null && includes.Length != 0)
        {
            foreach (var item in includes)
            {
                entity.Include(item);
            }
        }

        return entity;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.AddAsync(entity);
    }

    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includes)
    {
        var entity = GetEntity(includes);

        if (predicate != null)
        {
            entity.Where(predicate);
        }

        return entity;
    }

    public async Task<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        var entity = GetEntity(includes);

        return await entity.FirstOrDefaultAsync(x => !object.Equals(x.Id, null) && x.Id.Equals(id), cancellationToken);
    }

    public async Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        var entity = GetEntity(includes);

        return predicate != null
            ? await entity.SingleOrDefaultAsync(predicate, cancellationToken)
            : await entity.SingleOrDefaultAsync(cancellationToken);
    }

    public void Remove(TEntity entity)
    {
        _dbContext.Remove(entity);
    }

    public void RemoveMultiple(IEnumerable<TEntity> entities)
    {
        _dbContext.RemoveRange(entities);
    }

    public void Update(TEntity entity)
    {
        _dbContext.Update(entity);
    }

    public void UpdateMultiple(IEnumerable<TEntity> entities)
    {
        _dbContext.UpdateRange(entities);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
