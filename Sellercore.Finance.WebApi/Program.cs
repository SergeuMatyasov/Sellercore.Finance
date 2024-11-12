using HealthChecks.UI.Client;
using Sellercore.Finance.Infrastructure;
using Sellercore.Finance.Infrastructure.Extensions;
using Sellercore.Finance.WebApi.Extensions;
using Sellercore.Finance.WebApi.Middleware.ConditionalAuth;
using Sellercore.Finance.WebApi.Middleware.GlobalExceptionHandler;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Sellercore.Finance.Application;
using Sellercore.Finance.Domain;
using Sellercore.Finance.Ozon.Application;
using Sellercore.Finance.Ozon.Domain;
using Sellercore.Ozon.Shared.Application;
using Sellercore.Ozon.Shared.Domain;
using Sellercore.Shared.Application;
using Sellercore.Shared.Application.Settings;
using Sellercore.Shared.Domain;
using Sellercore.Shared.Infrastructure;
using Sellercore.Shared.Infrastructure.Extensions;
using Serilog;
using Shared.Application;
using Shared.Domain;
using Shared.Infrastructure;

namespace Sellercore.Finance.WebApi;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddSellercoreConsulConfiguration();
        builder.Configuration.AddMicroserviceNameConsulConfiguration();
        builder.Configuration.AddEnvironmentVariables();

        IConfiguration configuration = builder.Configuration;

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithHeaderCorrelationId()
            .CreateLogger();

        builder.Services.Configure<AppSettings>(configuration);

        builder.Services.LoadAllAssemblies();

        builder.Services.AddWebApi();

        builder.Services.AddSharedInfrastructure();
        builder.Services.AddLSellercoreSharedInfrastructure(configuration);
        builder.Services.AddInfrastructure(configuration);

        builder.Services.AddSharedApplication();
        builder.Services.AddSellercoreSharedApplication();
        builder.Services.AddApplication();
        builder.Services.AddCanvasWithImageOnStretcherApplication();
        builder.Services.AddCanvasWithImageOnStretcherSharedApplication();

        builder.Services.AddSharedDomain();
        builder.Services.AddSellercoreSharedDomain();
        builder.Services.AddDomain();
        builder.Services.AddCanvasWithImageOnStretcherSharedDomain();
        builder.Services.AddCanvasWithImageOnStretcherDomain();

        var app = builder.Build();

        // Конфигурация инфраструктуры
        //app.UseSerilogRequestLogging();
        app.UseRequestLogging();
        //todo: Включить при использовании базы данных
        //app.InitializeDatabase();

        // Конфигурация API и документации
        app.UseSwaggerWithVersioning();
        app.UseApiVersioning();

        // Безопасность и маршрутизация
        //app.UseHttpsRedirection();
        app.UseGlobalExceptionHandler();
        app.UseRouting();
        app.UseConditionalAuth();
        app.UseAuthentication();
        app.UseAuthorization();

        // Маршрутизация контроллеров и проверки состояния
        app.MapControllers();
#pragma warning disable ASP0014
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = reg => reg.Name != "masstransit-bus",
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        });
#pragma warning restore ASP0014

        // Запуск приложения
        try
        {
            Log.Information("The application starting up...");
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The application failed to start correctly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}