using Ozon.NET.Api;
using Sellercore.Finance.Ozon.Domain.Entities;
using Shared.Application.Common.Base;

namespace Sellercore.Finance.Ozon.Application.Interfaces.Repositories;

public interface IInternalOzonSellerRepository : IBaseRepository<InternalOzonSeller, int>
{
    /// <summary>
    /// Шифрует токен и устанавливает в сущность.
    /// </summary>
    /// <param name="sellerId">Идентификатор продавца Ozon.</param>
    /// <param name="token">НЕзашифрованный токен.</param>
    Task AddEncryptedSellerAsync(int sellerId, string token, CancellationToken cancellationToken);

    /// <summary>
    /// Извлекает продавца из базы и расшифровывает данные.
    /// </summary>
    /// <returns>Продавец с расшифрованными данными.</returns>
    Task<OzonSeller> GetDecryptedOzonSellerAsync(int sellerId, CancellationToken cancellationToken = default);
}