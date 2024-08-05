using Microsoft.EntityFrameworkCore;

namespace WebAppHero.Domain.Abstractions;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbContext GetDbContext();
}
