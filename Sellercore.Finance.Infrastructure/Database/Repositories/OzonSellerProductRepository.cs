using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ozon.NET.Api.UpdateAndUploadProducts.ProductList;
using Sellercore.Finance.Ozon.Application.Interfaces.Repositories;
using Sellercore.Finance.Ozon.Domain.Entities;
using Shared.Infrastructure.Base;

namespace Sellercore.Finance.Infrastructure.Database.Repositories;

public class OzonSellerProductRepository(
    MainContext context,
    ILogger<OzonSellerProductRepository> logger)
    : BaseRepository<OzonSellerProduct, Guid>(context), IOzonSellerProductRepository
{
    public async Task TryAddNewProductsAsync(int sellerId, List<OzonProduct> products,
        CancellationToken cancellationToken = default)
    {
        var existingOfferIds = await _dbSet
            .Where(e => e.SellerId == sellerId)
            .Where(e => products.Select(x => x.OfferId).Contains(e.OfferId))
            .Select(e => e.OfferId)
            .ToListAsync(cancellationToken);

        // Фильтруем сущности, которых еще не существуют в базе данных
        var newProducts = products
            .Where(e => !existingOfferIds.Contains(e.OfferId))
            .ToList();

        // Если есть новые сущности, добавляем их
        if (newProducts.Any())
        {
            await _dbSet.AddRangeAsync(newProducts.Select(e => new OzonSellerProduct
            {
                SellerId = sellerId,
                OfferId = e.OfferId,
                ProductId = e.ProductId
            }), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            logger.LogInformation("{Count} new products added for seller {SellerId}", 
                newProducts.Count, sellerId);
        }
    }
}