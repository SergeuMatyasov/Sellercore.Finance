using Sellercore.Finance.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Sellercore.Finance.Infrastructure.Database;

public class MainContext(DbContextOptions<MainContext> options, ILogger<MainContext> logger)
    : DbContext(options),
        IDbContext
{
    //public DbSet<Entity> Images { get; set; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation($"Saving changes to database. {nameof(MainContext)}.");

        await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await base.Database.BeginTransactionAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.ApplyConfiguration(new LeoImageConfiguration());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .LogTo(sql => logger.LogInformation(sql)) // Настройка логирования SQL запросов
                .EnableSensitiveDataLogging(); // Включение логирования чувствительных данных (опционально, используйте с осторожностью)
        }
    }
}