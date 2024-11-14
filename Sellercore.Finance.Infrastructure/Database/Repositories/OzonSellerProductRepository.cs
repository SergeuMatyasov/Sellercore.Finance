using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ozon.NET.Api.UpdateAndUploadProducts.ProductList;
using Sellercore.Finance.Ozon.Application.Interfaces.Repositories;
using Sellercore.Finance.Ozon.Domain.Entities;
using Sellercore.Finance.Ozon.Domain.Models;
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

    public async Task SetProductsCostAsync(int sellerId, List<OzonSellerProductCostModel> productsCostData,
        CancellationToken cancellationToken = default)
    {
        // Создаем словарь для быстрого доступа к стоимости по OfferId
        var productsCostDictionary = productsCostData.ToDictionary(p => p.OfferId, p => p.UnitCost);

        // Загружаем продукты, которые нужно обновить
        var productsToUpdate = await _dbSet
            .Where(p => p.SellerId == sellerId)
            .Where(p => productsCostDictionary.Keys.Contains(p.OfferId))
            .ToListAsync(cancellationToken);

        // Обновляем стоимость продуктов, используя словарь
        foreach (var productToUpdate in productsToUpdate)
        {
            if (productsCostDictionary.TryGetValue(productToUpdate.OfferId, out var unitCost))
            {
                productToUpdate.Cost = unitCost;
            }
        }

        // Сохраняем изменения в базе данных
        await SaveChangesAsync(cancellationToken);
    }
}