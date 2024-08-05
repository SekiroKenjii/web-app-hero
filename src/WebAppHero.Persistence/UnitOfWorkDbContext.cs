using Microsoft.EntityFrameworkCore;
using WebAppHero.Domain.Abstractions;

namespace WebAppHero.Persistence;

public sealed class UnitOfWorkDbContext<TContext>(TContext dbContext) : IUnitOfWorkDbContext<TContext>
    where TContext : DbContext
{
    public TContext GetDbContext()
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
