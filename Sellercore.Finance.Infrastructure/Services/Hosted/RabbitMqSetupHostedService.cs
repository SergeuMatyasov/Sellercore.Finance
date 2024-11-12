using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Application.Common.Interfaces;

namespace Sellercore.Finance.Infrastructure.Services.Hosted;

public class RabbitMqSetupHostedService(IServiceScopeFactory scopeFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var rabbitMqService = scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
        
        //rabbitMqService.DeclareQueue("example-queue");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        
    }
}