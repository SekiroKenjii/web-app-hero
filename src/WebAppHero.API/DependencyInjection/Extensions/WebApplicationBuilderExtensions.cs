using Serilog;

namespace WebAppHero.API.DependencyInjection.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureApplicationLogger(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        builder.Host.UseSerilog((context, loggerConfig) => {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
        });
    }
}
