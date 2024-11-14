using System.Text.Json.Serialization;
using MediatR;
using Sellercore.Finance.Ozon.Application.Interfaces.Repositories;
using Sellercore.Finance.Ozon.Domain.Models;
using Shared.Domain.Interfaces.CQRS;

namespace Sellercore.Finance.Ozon.Application.UseCases.Products.Commands.SetOzonSellerProductsCost;

public class SetOzonSellerProductsCostCommand(int ozonSellerId, List<OzonSellerProductCostModel> productCostData)
    : ICommand<Unit>
{
    /// <summary>
    /// Идентификатор продавца Ozon.
    /// </summary>
    [JsonPropertyName("ozon_seller_id")]
    public int OzonSellerId { get; set; } = ozonSellerId;

    /// <summary>
    /// Данные о сеестоимости товаров.
    /// </summary>
    [JsonPropertyName("product_cost_data")]
    public List<OzonSellerProductCostModel> ProductCostData { get; set; } = productCostData;
}

public class SetOzonSellerProductsCostCommandHandler(IOzonSellerProductRepository ozonSellerProductRepository)
    : ICommandHandler<SetOzonSellerProductsCostCommand, Unit>
{
    public async Task<Unit> Handle(SetOzonSellerProductsCostCommand request, CancellationToken cancellationToken)
    {
        await ozonSellerProductRepository.SetProductsCostAsync(request.OzonSellerId, request.ProductCostData,
            cancellationToken);

        return default;
    }
}