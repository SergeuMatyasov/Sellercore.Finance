using Shared.Domain.Common;

namespace Sellercore.Finance.Ozon.Domain.Entities;

public class OzonSellerProduct : BaseEntity<Guid>
{
    public int SellerId { get; set; }
    public InternalOzonSeller Seller { get; set; } = null!;
    
    public string OfferId { get; set; } = null!;
    public int? ProductId { get; set; }
    public int? Sku { get; set; }
    
    /// <summary>
    /// Себестоимость товара.
    /// </summary>
    public float? Cost { get; set; }
}