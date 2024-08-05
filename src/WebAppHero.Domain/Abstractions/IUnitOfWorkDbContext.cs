using Microsoft.EntityFrameworkCore;

namespace WebAppHero.Domain.Abstractions;

public interface IUnitOfWorkDbContext<TContext> : IAsyncDisposable
    where TContext : DbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    TContext GetDbContext();
}
