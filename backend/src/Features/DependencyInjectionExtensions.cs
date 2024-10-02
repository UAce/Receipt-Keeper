using System.Data;
using System.Reflection;
using Domain.Interfaces;
using Features.Endpoints;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Features;

public static class DependencyInjectionExtensions
{
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