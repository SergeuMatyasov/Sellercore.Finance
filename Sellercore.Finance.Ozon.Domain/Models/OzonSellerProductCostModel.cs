using System.Text.Json.Serialization;

namespace Sellercore.Finance.Ozon.Domain.Models;

public class OzonSellerProductCostModel(string offerId, float unitCost)
{
    /// <summary>
    /// Артикул товара.
    /// </summary>
    [JsonPropertyName("offer_id")]
    public string OfferId { get; set; } = offerId;

    /// <summary>
    /// Себестоимость.
    /// </summary>
    [JsonPropertyName("unit_cost")]
    public float UnitCost { get; set; } = unitCost;
}