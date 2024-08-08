using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAppHero.API.Abstractions;
using ProductSvcV1 = WebAppHero.Contract.Services.V1.Product;

namespace WebAppHero.API.Endpoints.Products;

public class ProductApi : ApiEndpoint, IEndpointModule
{
    public const string ProductApiUrl = $"{BaseUrl}/products";

    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        #region ========== API-VERSION-1 ==========

        var groupVersion1 = app.NewVersionedApi("products").MapGroup(ProductApiUrl).HasApiVersion(1);

        groupVersion1.MapPost("/for-testing", CreateProductsForTestingV1);
        groupVersion1.MapPost(string.Empty, CreateProductV1);
        groupVersion1.MapGet(string.Empty, GetProductsV1);
        groupVersion1.MapGet("{productId}", GetProductByIdV1);
        groupVersion1.MapPut("{productId}", UpdateProductV1);
        groupVersion1.MapDelete("{productId}", DeleteProductV1);

        #endregion
    }

    #region ========== API-VERSION-1 ==========

    public static async Task<IResult> CreateProductV1(ISender sender, [FromBody] ProductSvcV1.Command.CreateProductCommand request)
    {
        var result = await sender.Send(request);

        return HandleResult(result);
    }

    public static async Task<IResult> CreateProductsForTestingV1(ISender sender, [FromBody] ProductSvcV1.Command.CreateProductsForTestingCommand request)
    {
        var result = await sender.Send(request);

        return HandleResult(result);
    }

    public static async Task<IResult> GetProductsV1(ISender sender)
    {
        var result = await sender.Send(new ProductSvcV1.Query.GetProductsQuery());

        return HandleResult(result);
    }

    public static async Task<IResult> GetProductByIdV1(ISender sender, Guid productId)
    {
        var result = await sender.Send(new ProductSvcV1.Query.GetProductByIdQuery(productId));

        return HandleResult(result);
    }

    public static async Task<IResult> UpdateProductV1(ISender sender, Guid productId, [FromBody] ProductSvcV1.Command.UpdateProductCommand request)
    {
        var result = await sender.Send(new ProductSvcV1.Command.UpdateProductCommand(
            Id: productId,
            Name: request.Name,
            Price: request.Price,
            Description: request.Description
        ));

        return HandleResult(result);
    }

    public static async Task<IResult> DeleteProductV1(ISender sender, Guid productId)
    {
        var result = await sender.Send(new ProductSvcV1.Command.DeleteProductCommand(productId));

        return HandleResult(result);
    }

    #endregion
}
