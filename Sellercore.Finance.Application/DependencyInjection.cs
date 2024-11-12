using Microsoft.Extensions.DependencyInjection;

namespace Sellercore.Finance.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        return services;
    }
    
    public static void LoadApplicationAssembly(this IServiceCollection services)
    {
        
    }
}