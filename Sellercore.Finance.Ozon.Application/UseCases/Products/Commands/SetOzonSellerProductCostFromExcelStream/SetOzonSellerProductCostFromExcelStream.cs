using MediatR;
using Microsoft.AspNetCore.Http;
using Sellercore.Finance.Ozon.Application.Interfaces.Repositories;
using Sellercore.Finance.Ozon.Application.UseCases.Products.Queries.ParseProductCostExcel;
using Sellercore.Finance.Ozon.Domain.Models;
using Sellercore.Finance.Ozon.Domain.Models.ExcelModels;
using Shared.Domain.Interfaces.CQRS;

namespace Sellercore.Finance.Ozon.Application.UseCases.Products.Commands.SetOzonSellerProductCostFromExcelStream;

public class SetOzonSellerProductCostFromExcelStreamCommand(IFormFile excelStream, int sellerId) : ICommand<Unit>
{
    public int SellerId { get; set; } = sellerId;
    public IFormFile ExcelFile { get; set; } = excelStream;
}

public class SetOzonSellerProductCostFromExcelStreamCommandHandler(
    IOzonSellerProductRepository ozonSellerProductRepository,
    ISender sender)
    : ICommandHandler<SetOzonSellerProductCostFromExcelStreamCommand, Unit>
{
    private int sellerId;
    private IFormFile excelFile;
    
    public async Task<Unit> Handle(SetOzonSellerProductCostFromExcelStreamCommand request,
        CancellationToken cancellationToken)
    {
        sellerId = request.SellerId;
        excelFile = request.ExcelFile;
        
        using var stream = new MemoryStream();
        await excelFile.CopyToAsync(stream, cancellationToken);

        List<ProductCostExcelModel> typedRows =
            await sender.Send(new ParseProductCostExcelQuery(stream), cancellationToken);

        var cost = typedRows
            .Select(p => new OzonSellerProductCostModel(p.OfferId, p.Cost))
            .ToList();

        await ozonSellerProductRepository.SetProductsCostAsync(sellerId, cost, cancellationToken);

        return default;
    }
}