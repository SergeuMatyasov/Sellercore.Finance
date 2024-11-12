using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sellercore.Finance.Infrastructure.Health;

public class SeqHealthCheck(string seqUri) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using var httpClient = new HttpClient();
        try
        {
            // Обычно доступ к API Seq можно проверить через URL /api
            var response = await httpClient.GetAsync($"{seqUri}/api", cancellationToken);
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