using Microsoft.AspNetCore.Http;
using WebAppHero.Contract.Abstractions.Messages;
using WebAppHero.Contract.Abstractions.Shared;
using WebAppHero.Contract.Services.V1.Product;
using WebAppHero.Domain.Abstractions;

namespace WebAppHero.Application.UseCases.V1.Commands.Product;

public sealed class CreateProductsForTestingCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<Command.CreateProductsForTestingCommand>
{
    public async Task<RequestHandlerResult<Result>> Handle(Command.CreateProductsForTestingCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Product> products = [];

        for (var i = 0; i < 1000; i++)
        {
            products.Add(Domain.Entities.Product.Create(
                id: Guid.NewGuid(),
                name: $"product name {i}",
                price: 1000,
                description: $"product description {i}"
            ));
        }

        await unitOfWork.GetDbContext().AddRangeAsync(products, cancellationToken);

        return RequestHandlerResult<Result>.Create(Result.Success(), StatusCodes.Status201Created);
    }
}
