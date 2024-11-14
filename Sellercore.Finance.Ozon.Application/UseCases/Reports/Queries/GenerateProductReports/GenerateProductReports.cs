using MediatR;
using Ozon.NET.Api.OrderProcessingFbsAndRealFbs.GetPostingListFbs;
using Ozon.NET.Models.Reports;
using Sellercore.Finance.Ozon.Application.UseCases.Products.Queries.GetProductCost;
using Shared.Domain.Interfaces.CQRS;

namespace Sellercore.Finance.Ozon.Application.UseCases.Reports.Queries.GenerateProductReports;

public class GenerateProductReportsQuery(int sellerId, ShipmentDetails shipmentDetails)
    : IQuery<List<OzonProductReport>>
{
    public int SellerId { get; set; } = sellerId;
    public ShipmentDetails ShipmentDetails { get; set; } = shipmentDetails;
}

public class GenerateProductReportsQueryHandler(ISender sender)
    : IQueryHandler<GenerateProductReportsQuery, List<OzonProductReport>>
{
    private readonly List<OzonProductReport> reports = [];
    private ShipmentDetails shipmentDetails;
    private float taxPercent => throw new NotImplementedException();

    public async Task<List<OzonProductReport>> Handle(GenerateProductReportsQuery request,
        CancellationToken cancellationToken)
    {
        shipmentDetails = request.ShipmentDetails;

        for (int i = 0; i < shipmentDetails.Products.Count; i++)
        {
            FinancialProduct finInfoProduct = shipmentDetails.FinancialData.Products[i];

            string offerId = shipmentDetails.Products[i].OfferId;
            int price = int.Parse(shipmentDetails.Products[i].Price.Split(".")[0]);
            float payout = (float)finInfoProduct.Payout;
            float productCost =
                await sender.Send(new GetProductCostQuery(request.SellerId, offerId), cancellationToken);
            float taxRub = CalcTaxRub(payout);
            float ozonCommissionRub = Math.Abs((float)finInfoProduct.CommissionAmount);
            float salePrice = shipmentDetails.FinancialData.Products[i].CalcSalePrice();
            float acquiring = (float)(salePrice * 0.015);
            float profit = payout - taxRub - productCost - acquiring;

            reports.Add(new OzonProductReport
            {
                PercentMargin = CalcPercentMargin(profit, salePrice),
                Income = new OzonIncomeReport
                {
                    SellerPrice = price,
                    SalePrice = salePrice,
                    OzonPayout = payout,
                    Profit = profit,
                    Total = salePrice
                },
                Expense = new OzonExpenseReport
                {
                    UnitCost = productCost,
                    OzonCommissionRub = ozonCommissionRub,
                    Tax = taxRub,
                    Acquiring = acquiring,
                    Total = productCost + ozonCommissionRub + taxRub + acquiring
                }
            });
        }

        return reports;
    }

    /// <summary>
    /// Посчитать налог.
    /// </summary>
    private float CalcTaxRub(float totalPrice)
    {
        return totalPrice * taxPercent;
    }

    /// <summary>
    /// Посчитать маржинальность.
    /// </summary>
    private float CalcPercentMargin(float profit, float salePrice)
    {
        return profit / salePrice * 100;
    }
}