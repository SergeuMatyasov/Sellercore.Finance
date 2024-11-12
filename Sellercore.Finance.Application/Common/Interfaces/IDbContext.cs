using Sellercore.Finance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Domain.Interfaces.CustomServiceLifetimes;

namespace Sellercore.Finance.Application.Common.Interfaces;

public interface IDbContext : IScopedCustomService
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}