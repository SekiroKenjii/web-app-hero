using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WebAppHero.Domain.Abstractions;
using WebAppHero.Persistence;

namespace WebAppHero.Application.Behaviors;

public class TransactionPipelineBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork, ApplicationDbContext dbContext)
   : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!IsCommand())
        {
            return await next();
        }

        #region ========== SQL-SERVER-STRATEGY-1 ==========

        var strategy = dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () => {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            var response = await next();

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return response;
        });

        #endregion

        #region ========== SQL-SERVER-STRATEGY-2 ==========

        // using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        // var response = await next();

        // await unitOfWork.SaveChangesAsync(cancellationToken);
        // transaction.Complete();
        // await unitOfWork.DisposeAsync();

        // return response;

        #endregion
    }

    private static bool IsCommand()
    {
        return typeof(TRequest).Name.EndsWith("Command");
    }
}
