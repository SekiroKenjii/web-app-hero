using WebAppHero.Contract.Abstractions.Messages;
using static WebAppHero.Contract.Services.V1.Product.Response;

namespace WebAppHero.Contract.Services.V1.Product;

public static class Query
{
    public record GetProductsQuery() : IQuery<List<ProductResponse>>;

    public record GetProductByIdQuery(Guid Id) : IQuery<ProductResponse>;
}
