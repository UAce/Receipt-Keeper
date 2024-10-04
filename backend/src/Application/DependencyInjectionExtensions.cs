using System.Collections.Immutable;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using Application.Endpoints;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using Infrastructure.Repositories.Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAuthorizations(this IServiceCollection services)
    {
        services
            .AddAuthorizationBuilder()
            .SetDefaultPolicy(
                new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("Firebase")
                    .RequireAuthenticatedUser()
                    .Build()
            );

        return services;
    }

    public static IServiceCollection AddAuthentications(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        var configuration = builder.Configuration;
        var firebaseProjectId = configuration["Firebase:projectId"];
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                "Firebase",
                options =>
                {
                    options.Authority = $"https://securetoken.google.com/{firebaseProjectId}";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = $"https://securetoken.google.com/{firebaseProjectId}",
                        ValidAudience = $"{firebaseProjectId}",
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnForbidden = context =>
                        {
                            Console.WriteLine("Forbidden: " + context.Request.Path);
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            // Log the exception or take any action you want
                            Console.WriteLine(
                                "Authentication failed: " + context.Exception.Message
                            );
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = async context =>
                        {
                            var userRepository =
                                context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

                            var identityId = context
                                .Principal?.FindFirst(ClaimTypes.NameIdentifier)
                                ?.Value;

                            Console.WriteLine("Retrieving CurrentUser");
                            CurrentUser? currentUser =
                                identityId == null
                                    ? null
                                    : await userRepository.GetUserAsync(identityId);

                            if (currentUser != null)
                            {
                                // This will be used in the HttpContextService
                                context.HttpContext.Items["CurrentUser"] = currentUser;
                            }

                            return;
                        },
                    };
                }
            );

        return services;
    }

    public static IServiceCollection AddPersistences(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        string dbConnectionString =
            builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is null."
            );

        services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(dbConnectionString));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReceiptRepository, ReceiptRepository>();
        services.AddScoped<IMerchantRepository, MerchantRepository>();

        return services;
    }

    public static void AddValidatorsFromAssembly(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        // Scan for all types in the specified assembly
        var validatorTypes = assembly
            .GetTypes()
            .Where(t =>
                t.IsClass
                && !t.IsAbstract
                && t.GetInterfaces()
                    .Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)
                    )
            );

        foreach (var validatorType in validatorTypes)
        {
            // Register the validator as a transient service
            var interfaceType = validatorType
                .GetInterfaces()
                .First(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)
                );

            services.AddTransient(interfaceType, validatorType);
        }
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes.Where(type => type.ImplementedInterfaces.Contains(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<
            IEnumerable<IEndpoint>
        >();

        foreach (IEndpoint endpoint in endpoints)
        {
            var apiVersionGroup = endpoint.MapApiVersion(app);
            var resourceGroup = endpoint.MapResource(apiVersionGroup);
            endpoint.MapRoute(resourceGroup);
        }

        return app;
    }
}
