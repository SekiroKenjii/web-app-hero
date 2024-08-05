namespace WebAppHero.API.Abstractions;

public interface IEndpointModule
{
    void AddEndpoints(IEndpointRouteBuilder app);
}
