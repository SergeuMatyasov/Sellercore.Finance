using MediatR;
using Microsoft.Extensions.Logging;
using Ozon.NET.Api;
using Ozon.NET.Enums;
using Ozon.NET.Services.Products;
using Sellercore.Finance.Ozon.Application.Interfaces.Repositories;
using Shared.Domain.Interfaces.CQRS;

namespace Sellercore.Finance.Ozon.Application.UseCases.Products.Commands.SyncOzonProducts;

/// <summary>
/// Синхранизирует наличие товаров в кабинете Ozon и базе сервиса.
/// </summary>
public class SyncOzonProductsCommand(int sellerId) : ICommand<Unit>
{
    public int SellerId { get; set; } = sellerId;
}

public class SyncOzonProductsCommandHandler(
    ILogger<SyncOzonProductsCommandHandler> logger,
    IInternalOzonSellerRepository internalOzonSellerRepository,
    IOzonSellerProductRepository ozonSellerProductRepository)
    : ICommandHandler<SyncOzonProductsCommand, Unit>
{
    private int sellerId;
    
    public async Task<Unit> Handle(SyncOzonProductsCommand request, CancellationToken cancellationToken)
    {
        sellerId = request.SellerId;

        OzonSeller seller = await internalOzonSellerRepository
            .GetDecryptedOzonSellerAsync(sellerId, cancellationToken);
        IOzonProductService ozonProductService = new OzonProductService(seller, logger);

        var allProducts = await ozonProductService
            .FindProductsAsync(ProductVisibility.ALL, cancellationToken);

        await ozonSellerProductRepository.TryAddNewProductsAsync(sellerId, allProducts, cancellationToken);
        
        logger.LogInformation("Ozon products for {SellerId} synced.", sellerId);

        return default;
    }
}