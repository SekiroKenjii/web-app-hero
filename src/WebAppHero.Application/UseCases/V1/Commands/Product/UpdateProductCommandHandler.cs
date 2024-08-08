using Microsoft.AspNetCore.Http;
using WebAppHero.Contract.Abstractions.Messages;
using WebAppHero.Contract.Abstractions.Shared;
using WebAppHero.Contract.Services.V1.Product;
using WebAppHero.Domain.Abstractions.Repositories;

namespace WebAppHero.Application.UseCases.V1.Commands.Product;

public sealed class UpdateProductCommandHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepository)
    : ICommandHandler<Command.UpdateProductCommand>
{
    public async Task<RequestHandlerResult<Result>> Handle(Command.UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.FindByIdAsync(request.Id);

        if (product == null)
        {
            return RequestHandlerResult<Result>.Create(Result.Failure(Error.NullValue), StatusCodes.Status404NotFound);
        }

        product.Update(request.Name, request.Price, request.Description);

        return RequestHandlerResult<Result>.Create(Result.Success(), StatusCodes.Status204NoContent);
    }
}
