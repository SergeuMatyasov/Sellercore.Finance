using Sellercore.Finance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Sellercore.Finance.Ozon.Domain.Entities;
using Shared.Domain.Interfaces.CustomServiceLifetimes;

namespace Sellercore.Finance.Application.Common.Interfaces;

public interface IDbContext : IScopedCustomService
{
    public DbSet<InternalOzonSeller> InternalOzonSellers { get; set; }
    public DbSet<OzonSellerProduct> OzonSellerProducts { get; set; }
    
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}