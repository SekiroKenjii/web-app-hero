using Asp.Versioning;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using WebAppHero.API.Attributes;
using WebAppHero.API.DependencyInjection.Options;
using WebAppHero.API.Middlewares;
using WebAppHero.Application.DependencyInjection.Extensions;
using WebAppHero.Infrastructure.DependencyInjection.Extensions;
using WebAppHero.Infrastructure.DependencyInjection.Options;
using WebAppHero.Persistence.DependencyInjection.Extensions;

namespace WebAppHero.API.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();
        services.AddScoped<CustomJwtBearerEvents>();

        services
            .AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.RequireAuthenticatedSignIn = false;
            })
            .AddJwtBearer(options => {
                var isProduction = environment.IsProduction();

                var jwtOption = new JwtOption();
                configuration.GetSection(nameof(JwtOption)).Bind(jwtOption);

                ArgumentNullException.ThrowIfNull(jwtOption.SecretKey);
                var key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = isProduction,
                    ValidateAudience = isProduction,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOption.Issuer,
                    ValidAudience = jwtOption.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents {
                    OnAuthenticationFailed = context => {
                        if (context.Response.HasStarted)
                        {
                            return Task.CompletedTask;
                        }

                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("IS-TOKEN-EXPIRED", "true");
                            context.Response.ContentType = "application/json";

                            return context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                                title = "Authentication Error",
                                status = StatusCodes.Status401Unauthorized,
                                detail = "Token has been expired!",
                                errors = Enumerable.Empty<object>(),
                            }));
                        }

                        return Task.CompletedTask;
                    },
                    OnChallenge = context => {
                        if (context.Response.HasStarted)
                        {
                            return Task.CompletedTask;
                        }

                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        return context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                            title = "Authentication Error",
                            status = StatusCodes.Status401Unauthorized,
                            detail = "You are not authorized!",
                            errors = Enumerable.Empty<object>(),
                        }));
                    },
                    OnForbidden = context => {
                        if (context.Response.HasStarted)
                        {
                            return Task.CompletedTask;
                        }

                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        return context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                            title = "Authentication Error",
                            status = StatusCodes.Status403Forbidden,
                            detail = "You are not authorized to access this resource!",
                            errors = Enumerable.Empty<object>(),
                        }));
                    }
                };

                options.EventsType = typeof(CustomJwtBearerEvents);
            });

        services
            .AddAuthorizationBuilder()
            .AddPolicy("RequireAuthenticatedUser", policy => {
                policy.RequireAuthenticatedUser();
            });
    }

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
        services.AddApplicationBehaviors();
        services.AddRequestValidators();
        services.AddAutoMapperProfiles();
    }

    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureServices();
        services.AddRedisInfrastructure(configuration);
    }

    public static void AddPersistenceLayer(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddApplicationInterceptors();
        services.AddSqlServer(environment);
        services.AddRepositories();
    }
}
