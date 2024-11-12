using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sellercore.Finance.Infrastructure.Database;
using Sellercore.Finance.Infrastructure.Health;
using Sellercore.Finance.Infrastructure.Services.Hosted;

namespace Sellercore.Finance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgres(configuration);
        services.AddHealthChecks(configuration);
        services.AddHostedService<RabbitMqSetupHostedService>();
        
        return services;
    }

    private static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration["Database:ConnectionString"]!;
        services.AddDbContext<MainContext>(options => options.UseNpgsql(connectionString));

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new SeqHealthCheck(configuration["Seq:Host"]!));

        services.AddHealthChecks()
            /*.AddNpgSql(
                configuration["Database:ConnectionString"]!,
                name: "Postgres",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "sql", "postgres" })*/
            .AddRabbitMQ(
                rabbitConnectionString:
                $"amqp://{configuration["RabbitMq:Username"]}:{configuration["RabbitMq:Password"]}@{configuration["RabbitMq:Host"]}:5672",
                name: "RabbitMQ",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "mq", "messaging", "rabbitmq" })
            .AddCheck<SeqHealthCheck>(
                "Seq",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "Seq", "logging" })
            /*.AddCheck<IdentityServiceHealthCheck>(
                "IdentityService",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "IdentityService", "auth" })*/;

        return services;
    }
    
    public static void LoadInfrastructureAssembly(this IServiceCollection services)
    {
        
    }
}