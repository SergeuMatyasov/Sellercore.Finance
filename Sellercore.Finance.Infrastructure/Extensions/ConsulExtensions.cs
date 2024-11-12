using Microsoft.Extensions.Configuration;
using Winton.Extensions.Configuration.Consul;

namespace Sellercore.Finance.Infrastructure.Extensions;

public static class ConsulExtensions
{
    public static ConfigurationManager AddMicroserviceNameConsulConfiguration(this ConfigurationManager configuration)
    {
        /*var consulUrl = configuration["Consul:Url"]!;

        configuration.AddConsul("", options =>
        {
            options.ConsulConfigurationOptions = cco => { cco.Address = new Uri(consulUrl); };
            options.Optional = true;
            options.ReloadOnChange = true;
        });
        */

        return configuration;
    }
}