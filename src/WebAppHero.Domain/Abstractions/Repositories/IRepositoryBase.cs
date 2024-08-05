using System.Linq.Expressions;

namespace WebAppHero.Domain.Abstractions.Repositories;

public interface IRepositoryBase<TEntity, in TKey>
    where TEntity : class // => In implementation, it should be EntityBase<TKey>
{
    Task<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

    IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includes);

    Task AddAsync(TEntity entity);

    void Update(TEntity entity);

    void UpdateMultiple(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveMultiple(IEnumerable<TEntity> entities);
}
