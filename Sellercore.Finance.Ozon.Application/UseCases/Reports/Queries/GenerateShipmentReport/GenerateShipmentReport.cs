using MediatR;
using Microsoft.Extensions.Logging;
using Ozon.NET.Api;
using Ozon.NET.Api.OrderProcessingFbsAndRealFbs.GetPostingListFbs;
using Ozon.NET.Enums;
using Ozon.NET.Models.Reports;
using Ozon.NET.Services.Shipment;
using Ozon.NET.Services.TransactionService;
using Sellercore.Finance.Ozon.Application.UseCases.Reports.Queries.GenerateProductReports;
using Sellercore.Ozon.Shared.Application.UseCases.Sellers.Queries;
using Shared.Domain.Exceptions;
using Shared.Domain.Extensions;
using Shared.Domain.Interfaces.CQRS;

namespace Sellercore.Finance.Ozon.Application.UseCases.Reports.Queries.GenerateShipmentReport;

public class GenerateShipmentReportQuery(int sellerId, string postingNumber) : IQuery<OzonShipmentReport>
{
    /// <summary>
    /// Идентификатор продавца Ozon.
    /// </summary>
    public int OzonSellerId { get; set; } = sellerId;

    /// <summary>
    /// Номер отправления.
    /// </summary>
    public string PostingNumber { get; set; } = postingNumber;
}

public class GenerateShipmentReportQueryHandler(
    ISender sender,
    ILogger<GenerateShipmentReportQueryHandler> logger)
    : IQueryHandler<GenerateShipmentReportQuery, OzonShipmentReport>
{
    private int sellerId;
    private string postingNumber;

    public async Task<OzonShipmentReport> Handle(GenerateShipmentReportQuery request,
        CancellationToken cancellationToken)
    {
        sellerId = request.OzonSellerId;
        postingNumber = request.PostingNumber;

        OzonSeller seller = await sender.Send(new GetOzonSellerQuery(sellerId), cancellationToken);

        IOzonShipmentService ozonShipmentService = new OzonShipmentService(seller, logger);
        ITransactionService transactionService = new TransactionService(seller, logger);

        var transactionList = await transactionService.GetTransactionListAsync(postingNumber, cancellationToken);
        ShipmentDetails shipmentDetails = (await ozonShipmentService
            .GetShipmentInfoWithAdditionalDataAsync(postingNumber, cancellationToken)).result;

        List<OzonProductReport> productReports =
            await sender.Send(new GenerateProductReportsQuery(sellerId, shipmentDetails), cancellationToken);
        
        float deliveryCost = transactionService.TryCalcDeliveryCost(transactionList)
                             ?? throw new NotFoundException
                                 ("The cost of delivery could not be calculated. No services found.");

        var report = new OzonShipmentReport
        {
            PostingNumber = postingNumber,
            Status = shipmentDetails.Status,
            ProductsQuantity = shipmentDetails.Products.Count,
            IsCanceled = shipmentDetails.Status == FbsShipmentStatus.Cancelled.GetDescription(),
            PercentMargin = 0,
            ProductReports = productReports,
            Income = OzonIncomeReport.AggregateReports(productReports.Select(r => r.Income).ToList()),
            Expense = OzonExpenseReport.AggregateReports(productReports.Select(r => r.Expense).ToList())
        };

        report.Expense.Delivery = deliveryCost;

        return report;
    }
}