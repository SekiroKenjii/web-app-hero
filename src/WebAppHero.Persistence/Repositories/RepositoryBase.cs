using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAppHero.Domain.Abstractions;
using WebAppHero.Domain.Abstractions.Repositories;
using WebAppHero.Persistence.Abstractions;

namespace WebAppHero.Persistence.Repositories;

public sealed class RepositoryBase<TEntity, TKey>(DbContext dbContext)
    : RepositoryAbstract<DbContext, TEntity, TKey>, IRepositoryBase<TEntity, TKey>, IDisposable
    where TEntity : EntityBase<TKey>
{
    private readonly DbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.AddAsync(entity);
    }

    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includes)
    {
        var entity = GetEntity(_dbContext, includes);

        if (predicate != null)
        {
            entity.Where(predicate);
        }

        return entity;
    }

    public async Task<TEntity?> FindByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes)
    {
        Expression<Func<TEntity, bool>> predicate = x => !Equals(x.Id, null) && x.Id.Equals(id);

        return await FirstOrDefaultAsync(_dbContext, predicate, includes);
    }

    public async Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includes)
    {
        return await SingleOrDefaultAsync(_dbContext, predicate, includes);
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
