using Shared.Domain.Common;

namespace Sellercore.Finance.Ozon.Domain.Entities;

public class InternalOzonSeller : BaseEntity<int>
{
    /// <summary>
    /// Токен (Храниться в зашифрованном виде)
    /// </summary>
    public string Token { get; set; }
    
    /// <summary>
    /// Товары продавца.
    /// </summary>
    public virtual ICollection<OzonSellerProduct>? Products { get; set; }
}