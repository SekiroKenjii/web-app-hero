using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAppHero.API.DependencyInjection.Options;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme {
                Description = @"<p>JWT Authorization header using the Bearer scheme.</br>
                               Enter 'Bearer' [space] and then your token in the text input below.</br>
                               Example: 'Bearer json-web-token'</p>",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement {{
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    },
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Name = "Authentication",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }});
        }

        options.MapType<DateOnly>(() => new() {
            Format = "date",
            Example = new OpenApiString(DateOnly.MinValue.ToString())
        });

        options.CustomOperationIds(type => type.ToString()?.Replace("+", "."));
    }

    private static OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo {
            Title = "WebAppHero.CleanArchitecture.Template",
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated) info.Description += " (deprecated)";

        return info;
    }
}
