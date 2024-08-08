using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAppHero.Domain.Abstractions;
using WebAppHero.Domain.Abstractions.Repositories;
using WebAppHero.Persistence.Abstractions;

namespace WebAppHero.Persistence.Repositories;

public sealed class RepositoryBaseDbContext<TContext, TEntity, TKey>(TContext? dbContext)
    : RepositoryAbstract<TContext, TEntity, TKey>, IRepositoryBaseDbContext<TContext, TEntity, TKey>, IDisposable
    where TContext : DbContext
    where TEntity : EntityBase<TKey>
{
    private readonly TContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.AddAsync(entity);
    }

    public IQueryable<TEntity> FindAll(
        Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[]? includes)
    {
        var entity = GetEntity(
            dbContext: _dbContext,
            isTracking: false,
            includes: includes
        );

        if (predicate != null)
        {
            entity.Where(predicate);
        }

        return entity;
    }

    public async Task<TEntity?> FindByIdAsync(
        TKey id,
        bool useCompiledQuery = true,
        params Expression<Func<TEntity, object>>[]? includes)
    {
        if (useCompiledQuery && includes?.Length == 0)
        {
            return await FirstOrDefaultByIdAsync(_dbContext, id);
        }

        return await GetEntity(_dbContext, true, includes)
            .FirstOrDefaultAsync(x => !Equals(x.Id, null) && x.Id.Equals(id));
    }

    public async Task<TEntity?> FindSingleAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool useCompiledQuery = true,
        params Expression<Func<TEntity, object>>[]? includes)
    {
        if (useCompiledQuery && includes?.Length == 0)
        {
            return await SingleOrDefaultAsync(_dbContext, predicate);
        }

        return await GetEntity(_dbContext, true, includes).SingleOrDefaultAsync(predicate);
    }

    public void Remove(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public void RemoveMultiple(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
    }

    public void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }

    public void UpdateMultiple(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
