using Microsoft.AspNetCore.Http;
using WebAppHero.Contract.Abstractions.Messages;
using WebAppHero.Contract.Abstractions.Shared;
using WebAppHero.Contract.Services.V1.Product;
using WebAppHero.Domain.Abstractions.Repositories;

namespace WebAppHero.Application.UseCases.V1.Commands.Product;

public sealed class CreateProductCommandHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepository)
    : ICommandHandler<Command.CreateProductCommand>
{
    public async Task<RequestHandlerResult<Result>> Handle(Command.CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Domain.Entities.Product.Create(Guid.NewGuid(), request.Name, request.Price, request.Description);

        await productRepository.AddAsync(product);

        return RequestHandlerResult<Result>.Create(Result.Success(), StatusCodes.Status201Created);
    }
}
