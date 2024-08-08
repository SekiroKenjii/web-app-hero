using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WebAppHero.Domain.Abstractions;
using WebAppHero.Domain.Abstractions.Repositories;
using WebAppHero.Domain.Entities.Identity;
using WebAppHero.Persistence.DependencyInjection.Options;
using WebAppHero.Persistence.Interceptors;
using WebAppHero.Persistence.Repositories;

namespace WebAppHero.Persistence.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationInterceptors(this IServiceCollection services)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
    }

    public static void AddSqlServer(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) => {
            var outboxInterceptor = provider.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            var auditableInterceptor = provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();
            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>().CurrentValue;

            #region ========== SQL-SERVER-STRATEGY-1 ==========

            if (environment.IsDevelopment() || environment.IsStaging())
            {
                builder
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .UseLazyLoadingProxies()
                    .UseSqlServer(
                        connectionString: configuration.GetConnectionString("SqlServerConnectionString"),
                        sqlServerOptionsAction: optionsBuilder => {
                            optionsBuilder
                                .ExecutionStrategy(dependencies => new SqlServerRetryingExecutionStrategy(
                                    dependencies: dependencies,
                                    maxRetryCount: 1,
                                    maxRetryDelay: TimeSpan.FromSeconds(0),
                                    errorNumbersToAdd: options.ErrorNumbersToAdd
                                ))
                                .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name);
                        })
                    .AddInterceptors(outboxInterceptor, auditableInterceptor);
            }

            #endregion

            #region ========== SQL-SERVER-STRATEGY-2 ==========

            if (environment.IsProduction())
            {
                builder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(
                        connectionString: configuration.GetConnectionString("SqlServerConnectionString"),
                        sqlServerOptionsAction: optionsBuilder => {
                            optionsBuilder
                                .ExecutionStrategy(dependencies => new SqlServerRetryingExecutionStrategy(
                                    dependencies: dependencies,
                                    maxRetryCount: options.MaxRetryCount,
                                    maxRetryDelay: options.MaxRetryDelay,
                                    errorNumbersToAdd: options.ErrorNumbersToAdd
                                ))
                                .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name);
                        })
                    .AddInterceptors(outboxInterceptor, auditableInterceptor);
            }

            #endregion
        });

        var passwordValidatorOptions =
            services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<PasswordValidatorOptions>>().CurrentValue;

        services
            .AddIdentityCore<AppUser>(options => {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.Password.RequireDigit = passwordValidatorOptions.RequiredDigitLength > 0;
                options.Password.RequireLowercase = passwordValidatorOptions.RequiredLowercaseLength > 0;
                options.Password.RequireUppercase = passwordValidatorOptions.RequiredUppercaseLength > 0;
                options.Password.RequireNonAlphanumeric = passwordValidatorOptions.RequiredNonAlphanumericLength > 0;
                options.Password.RequiredLength = passwordValidatorOptions.RequiredMinLength > 0
                    ? passwordValidatorOptions.RequiredMinLength
                    : 6;
                options.Password.RequiredUniqueChars = 1;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));

        services.AddTransient(typeof(IUnitOfWorkDbContext<>), typeof(UnitOfWorkDbContext<>));
        services.AddTransient(typeof(IRepositoryBaseDbContext<,,>), typeof(RepositoryBaseDbContext<,,>));
    }
}
