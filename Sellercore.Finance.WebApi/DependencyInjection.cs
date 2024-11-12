using Sellercore.Finance.Application;
using Sellercore.Finance.Ozon.Application;
using Sellercore.Finance.Ozon.Domain;
using Sellercore.Finance.Domain;
using Sellercore.Finance.Infrastructure;
using Sellercore.Shared.Infrastructure;
using Microsoft.Extensions.Options;
using Sellercore.Finance.WebApi.Configurations;
using Sellercore.Ozon.Shared.Application;
using Sellercore.Ozon.Shared.Domain;
using Sellercore.Shared.Application;
using Sellercore.Shared.Domain;
using Shared.Application;
using Shared.Domain;
using Shared.Infrastructure;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sellercore.Finance.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddCustomApiVersioning();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        
        return services;
    }

    /// <summary>
    /// Добавить версионирование Api
    /// </summary>
    private static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
    {
        // Настройка API Explorer для поддержки версионирования
        services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();
        services.AddApiVersioning();

        return services;
    }

    public static void LoadAllAssemblies(this IServiceCollection services)
    {
        services.LoadSharedInfrastructureAssembly();
        services.LoadSellercoreSharedInfrastructureAssembly();
        services.LoadInfrastructureAssembly();

        services.LoadSharedApplicationAssembly();
        services.LoadSellercoreSharedApplicationAssembly();
        services.LoadApplicationAssembly();
        services.LoadOzonApplicationAssembly();
        services.LoadOzonSharedApplicationAssembly();

        services.LoadSharedDomainAssembly();
        services.LoadSellercoreSharedDomainAssembly();
        services.LoadDomainAssembly();
        services.LoadOzonSharedDomainAssembly();
        services.LoadOzonDomainAssembly();
    }
}