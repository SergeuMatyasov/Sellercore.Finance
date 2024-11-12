using Microsoft.Extensions.DependencyInjection;

namespace Sellercore.Finance.Ozon.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCanvasWithImageOnStretcherApplication(this IServiceCollection services)
    {
        return services;
    }
    
    public static void LoadOzonApplicationAssembly(this IServiceCollection services)
    {
        
    }
}