namespace Sellercore.Finance.Ozon.Domain.Models.ExcelModels;

public class ProductCostExcelModel(string offerId, float cost)
{
    public string OfferId { get; set; } = offerId;
    public float Cost { get; set; } = cost;
}