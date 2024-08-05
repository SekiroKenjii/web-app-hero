using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WebAppHero.Application.Behaviors;
using WebAppHero.Application.Mappers;

namespace WebAppHero.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMediatRConfigurations(this IServiceCollection services)
    {
        services
            .AddMediatR(config => {
                config.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            })
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationDefaultPipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            //.AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformancePipelineBehavior<,>))
            //.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>))
            .AddValidatorsFromAssembly(Contract.AssemblyReference.Assembly, includeInternalTypes: true);
    }

    public static void AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MapperProfile));
    }
}
