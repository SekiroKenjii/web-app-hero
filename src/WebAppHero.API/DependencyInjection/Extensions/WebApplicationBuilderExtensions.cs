using Serilog;

namespace WebAppHero.API.DependencyInjection.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureApplicationLogger(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom
            .Configuration(builder.Configuration)
            .CreateLogger();

        builder.Logging.ClearProviders().AddSerilog();

        builder.Host.UseSerilog();
    }
}
