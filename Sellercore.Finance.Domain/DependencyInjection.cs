using Microsoft.Extensions.DependencyInjection;

namespace Sellercore.Finance.Domain;

/// <summary>
/// Общие зависимости для всех микросервисов (Не зависит от конкретного товара)
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services;
    }
    
    public static void LoadDomainAssembly(this IServiceCollection services)
    {
        
    }
}