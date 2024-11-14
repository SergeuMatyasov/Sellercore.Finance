using Ozon.NET.Api;
using Ozon.NET.Api.UpdateAndUploadProducts.ProductList;
using Sellercore.Finance.Ozon.Application.Interfaces.Repositories;
using Sellercore.Finance.Ozon.Domain.Entities;
using Sellercore.Shared.Application.Interfaces;
using Shared.Infrastructure.Base;

namespace Sellercore.Finance.Infrastructure.Database.Repositories;

public class InternalOzonSellerRepository(
    MainContext context,
    IYandexKeyManagementService yandexKeyManagementService)
    : BaseRepository<InternalOzonSeller, int>(context), IInternalOzonSellerRepository
{
    public async Task AddEncryptedSellerAsync(int sellerId, string token, CancellationToken cancellationToken)
    {
        InternalOzonSeller seller = new InternalOzonSeller
        {
            Id = sellerId,
            Token = await yandexKeyManagementService.EncryptAsync(token, cancellationToken)
        };

        await AddAndSaveChangesAsync(seller, cancellationToken);
    }

    public async Task<OzonSeller> GetDecryptedOzonSellerAsync(int sellerId,
        CancellationToken cancellationToken = default)
    {
        var seller = await GetByIdAsync(sellerId, cancellationToken);
        return new OzonSeller(sellerId, await yandexKeyManagementService.DecryptAsync(seller.Token, cancellationToken));
    }
}