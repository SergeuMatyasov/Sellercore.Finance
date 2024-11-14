using Shared.Domain.Interfaces.CQRS;

namespace Sellercore.Finance.Ozon.Application.UseCases.Products.Queries.GetProductCost;

/// <summary>
/// Получить себестоимость товара.
/// </summary>
public class GetProductCostQuery(int sellerId, string offerId) : IQuery<float>
{
    public int OzonSellerId { get; set; } = sellerId;
    public string OfferId { get; set; } = offerId;
}

public class GetProductCostQueryHandler : IQueryHandler<GetProductCostQuery, float>
{
    public Task<float> Handle(GetProductCostQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}