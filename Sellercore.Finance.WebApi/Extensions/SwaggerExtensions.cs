using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Sellercore.Finance.WebApi.Extensions;

public static class SwaggerExtensions
{
    public static void UseSwaggerWithVersioning(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;
        
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            
        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                config.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    }
}