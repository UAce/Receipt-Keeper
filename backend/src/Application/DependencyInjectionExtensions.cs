using System.Collections.Immutable;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Domain.Interfaces;
using Application.Endpoints;
using FirebaseAdmin.Auth;
using Infrastructure.Repositories;
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
        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes("Firebase")
            .RequireAuthenticatedUser()
            .Build());

        return services;
    }

    public static IServiceCollection AddAuthentications(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var firebaseProjectId = configuration["Firebase:projectId"];
        services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer("Firebase", options =>
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
                    Console.WriteLine("Authentication failed: " + context.Exception.Message);
                    return Task.CompletedTask;
                },
            };
        });

        return services;
    }

    public static IServiceCollection AddPersistences(this IServiceCollection services, WebApplicationBuilder builder)
    {
        string dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                                     ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is null.");

        services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(dbConnectionString));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

     public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        ServiceDescriptor[] serviceDescriptors = assembly.DefinedTypes.Where(type => type.ImplementedInterfaces.Contains(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.Map(app);
        }

        return app;
    }
}