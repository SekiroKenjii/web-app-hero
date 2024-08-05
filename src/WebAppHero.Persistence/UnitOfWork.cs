using Microsoft.EntityFrameworkCore;
using WebAppHero.Domain.Abstractions;

namespace WebAppHero.Persistence;

public sealed class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    public DbContext GetDbContext()
    {
        return dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await dbContext.DisposeAsync();
    }
}
