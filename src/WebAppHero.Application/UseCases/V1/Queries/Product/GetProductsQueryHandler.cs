using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebAppHero.Contract.Abstractions.Messages;
using WebAppHero.Contract.Abstractions.Shared;
using WebAppHero.Contract.Services.V1.Product;
using WebAppHero.Domain.Abstractions.Repositories;

namespace WebAppHero.Application.UseCases.V1.Queries.Product;

public sealed class GetProductsQueryHandler(
    IRepositoryBase<Domain.Entities.Product, Guid> productRepository,
    IMapper mapper) : IQueryHandler<Query.GetProductsQuery, List<Response.ProductResponse>>
{
    public async Task<RequestHandlerResult<Result<List<Response.ProductResponse>>>> Handle(Query.GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.FindAll().ToListAsync(cancellationToken);

        return RequestHandlerResult<Result<List<Response.ProductResponse>>>.Create(
            Result.Success(mapper.Map<List<Response.ProductResponse>>(products)),
            StatusCodes.Status200OK
        );
    }
}
