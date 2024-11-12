using Microsoft.Extensions.DependencyInjection;

namespace Sellercore.Finance.Ozon.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddCanvasWithImageOnStretcherDomain(this IServiceCollection services)
    {
        return services;
    }
    
    public static void LoadOzonDomainAssembly(this IServiceCollection services)
    {
        
    }
}