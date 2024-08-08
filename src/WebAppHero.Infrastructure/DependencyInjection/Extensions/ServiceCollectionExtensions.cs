using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAppHero.Application.Abstractions;
using WebAppHero.Infrastructure.Authentication;
using WebAppHero.Infrastructure.Caching;

namespace WebAppHero.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services
            .AddTransient<IJwtService, JwtService>()
            .AddTransient<ICacheService, CacheService>();
    }

    public static void AddRedisInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options => {
            var connectionString = configuration.GetConnectionString("RedisConnectionString");
            options.Configuration = connectionString;
        });
    }
}
