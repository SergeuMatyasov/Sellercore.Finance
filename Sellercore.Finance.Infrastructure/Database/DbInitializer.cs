using Microsoft.EntityFrameworkCore;

namespace Sellercore.Finance.Infrastructure.Database;

public static class DbInitializer
{
    public static void Initialize(MainContext context)
    {
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
}