using Ozon.NET.Api.UpdateAndUploadProducts.ProductList;
using Sellercore.Finance.Ozon.Domain.Entities;
using Shared.Application.Common.Base;

namespace Sellercore.Finance.Ozon.Application.Interfaces.Repositories;

public interface IOzonSellerProductRepository : IBaseRepository<OzonSellerProduct, Guid>
{
    /// <summary>
    /// Добавляет товары из Ozon в базу.
    /// </summary>
    /// <param name="sellerId">Идендификатор продавца Ozon.</param>
    /// <param name="products">Товары Ozon.</param>
    /// <remarks>Если товар уже есть в базе, то ничего не происходит.</remarks>
    Task TryAddNewProductsAsync(int sellerId, List<OzonProduct> products,
        CancellationToken cancellationToken = default);
}