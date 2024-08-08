using Microsoft.AspNetCore.Http;
using WebAppHero.Contract.Abstractions.Messages;
using WebAppHero.Contract.Abstractions.Shared;
using WebAppHero.Contract.Services.V1.Product;
using WebAppHero.Domain.Abstractions.Repositories;

namespace WebAppHero.Application.UseCases.V1.Commands.Product;

public sealed class DeleteProductCommandHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepository)
    : ICommandHandler<Command.DeleteProductCommand>
{
    public async Task<RequestHandlerResult<Result>> Handle(Command.DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.FindByIdAsync(request.Id);

        if (product == null)
        {
            return RequestHandlerResult<Result>.Create(Result.Failure(Error.NullValue), StatusCodes.Status404NotFound);
        }

        product.Delete();

        productRepository.Remove(product);

        return RequestHandlerResult<Result>.Create(Result.Success(), StatusCodes.Status200OK);
    }
}
