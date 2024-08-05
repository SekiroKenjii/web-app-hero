using WebAppHero.API.Abstractions;

namespace WebAppHero.API.Endpoints.Products;

public class ProductApi : ApiEndpoint, IEndpointModule
{
    public const string ProductApiUrl = $"{BaseUrl}/products";

    public void AddEndpoints(IEndpointRouteBuilder app)
    {
        #region ========== API-VERSION-1 ==========

        var groupVersion1 = app.NewVersionedApi("products").MapGroup(ProductApiUrl).HasApiVersion(1);

        groupVersion1.MapGet(string.Empty, () => "GET /v1/products");

        #endregion

        #region ========== API-VERSION-2 ==========

        var groupVersion2 = app.NewVersionedApi("products").MapGroup(ProductApiUrl).HasApiVersion(2);

        groupVersion2.MapGet(string.Empty, () => "GET /v2/products");

        #endregion
    }
}
