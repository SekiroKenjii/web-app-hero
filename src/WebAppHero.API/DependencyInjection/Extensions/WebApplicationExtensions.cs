using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using WebAppHero.API.Abstractions;

namespace WebAppHero.API.DependencyInjection.Extensions;

public static class WebApplicationExtensions
{
    public static void MapApiEndpoints(this WebApplication app)
    {
        var endpointModules = AssemblyReference.Assembly
            .GetTypes()
            .Where(x => typeof(IEndpointModule).IsAssignableFrom(x) && x.IsClass)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointModule>();

        foreach (var endpointModule in endpointModules)
        {
            endpointModule.AddEndpoints(app);
        }
    }

    public static void ConfigureSwagger(this WebApplication app, IWebHostEnvironment environment)
    {
        if (environment.IsProduction())
        {
            return;
        }

        app.UseSwagger();
        app.UseSwaggerUI(options => {
            foreach (var version in app.DescribeApiVersions().Select(x => x.GroupName))
            {
                options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
            }

            // options.EnableTryItOutByDefault();
            options.DisplayRequestDuration();
            options.DocExpansion(DocExpansion.None);
        });

        app.MapGet("/", () => Results.Redirect("/swagger/index.html")).WithTags(string.Empty);
    }

    public static async Task RunApplicationAsync(this WebApplication app)
    {
        try
        {
            await app.RunAsync();

            Log.Information("Server's stopped cleanly.");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An unhandled exception occurred while attempting to bootstrap the server.");

            await app.StopAsync();
        }
        finally
        {
            await Log.CloseAndFlushAsync();

            await app.DisposeAsync();
        }
    }
}
