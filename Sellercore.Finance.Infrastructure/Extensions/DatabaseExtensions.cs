using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sellercore.Finance.Infrastructure.Database;
using Serilog;

namespace Sellercore.Finance.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static void InitializeDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<MainContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An error occurred while ImageLibraryContext initialization");
            throw;
        }
    }
}