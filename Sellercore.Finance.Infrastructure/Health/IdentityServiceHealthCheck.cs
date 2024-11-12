using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sellercore.Finance.Infrastructure.Health;

public class IdentityServiceHealthCheck(IConfiguration config) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        using var httpClient = new HttpClient();
        try
        {
            // Обычно доступ к API Seq можно проверить через URL /api
            var response = await httpClient.GetAsync($"{config["IdentityService:Host"]}/api", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Seq check failed with an exception.", ex);
        }
    }
}