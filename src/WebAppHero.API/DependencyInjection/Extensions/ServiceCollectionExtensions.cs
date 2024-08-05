using Asp.Versioning;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebAppHero.API.DependencyInjection.Options;
using WebAppHero.Application.DependencyInjection.Extensions;
using WebAppHero.Persistence.DependencyInjection.Extensions;

namespace WebAppHero.API.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApiConfigurations(this IServiceCollection services)
    {
        services
            .Configure<RouteOptions>(options => {
                options.LowercaseUrls = true;
            })
            .AddEndpointsApiExplorer()
            .AddApiVersioning(options => {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services
            .AddSwaggerGenNewtonsoftSupport()
            .AddFluentValidationRulesToSwagger()
            .AddSwaggerGen();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    }

    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatRConfigurations();
        services.AddAutoMapperProfiles();
    }

    public static void AddPersistenceLayer(this IServiceCollection services)
    {
        services.AddInterceptors();
        services.AddSqlServer();
        services.AddRepositories();
    }
}