using AutoMapper;
using Microsoft.AspNetCore.Http;
using WebAppHero.Contract.Abstractions.Messages;
using WebAppHero.Contract.Abstractions.Shared;
using WebAppHero.Contract.Services.V1.Product;
using WebAppHero.Domain.Abstractions.Repositories;

namespace WebAppHero.Application.UseCases.V1.Queries.Product;

public sealed class GetProductByIdQueryHandler(
    IRepositoryBase<Domain.Entities.Product, Guid> productRepository,
    IMapper mapper) : IQueryHandler<Query.GetProductByIdQuery, Response.ProductResponse>
{
    public async Task<RequestHandlerResult<Result<Response.ProductResponse>>> Handle(Query.GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.FindByIdAsync(request.Id);

        if (product == null)
        {
            return RequestHandlerResult<Result<Response.ProductResponse>>.Create(
                Result.Failure<Response.ProductResponse>(Error.NullValue),
                StatusCodes.Status404NotFound
            );
        }

        return RequestHandlerResult<Result<Response.ProductResponse>>.Create(
            Result.Success(mapper.Map<Response.ProductResponse>(product)),
            StatusCodes.Status200OK
        );
    }
}
