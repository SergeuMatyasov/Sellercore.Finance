using ClosedXML.Excel;
using Sellercore.Finance.Ozon.Domain.Models.ExcelModels;
using Shared.Domain.Interfaces.CQRS;

namespace Sellercore.Finance.Ozon.Application.UseCases.Products.Queries.ParseProductCostExcel;

public class ParseProductCostExcelQuery(Stream excelStream) : IQuery<List<ProductCostExcelModel>>
{
    public Stream ExcelStream { get; set; } = excelStream;
}

public class ParseProductCostExcelQueryHandler
    : IQueryHandler<ParseProductCostExcelQuery, List<ProductCostExcelModel>>
{
    private Stream excelStream;
    
    public async Task<List<ProductCostExcelModel>> Handle(ParseProductCostExcelQuery request,
        CancellationToken cancellationToken)
    {
        excelStream = request.ExcelStream;
        
        excelStream.Position = 0;
        
        List<ProductCostExcelModel> products = [];

        using var workbook = new XLWorkbook(excelStream);
        var worksheet = workbook.Worksheet(1); // Используем первый лист
        var rows = worksheet.RangeUsed()!.RowsUsed().Skip(1); // Пропускаем заголовок

        foreach (var row in rows)
        {
            var product = new ProductCostExcelModel
            {
                OfferId = row.Cell(1).GetValue<string>(), // Первый столбец - Артикул
                Cost = row.Cell(2).GetValue<float>()    // Второй столбец - Себестоимость
            };
                
            products.Add(product);
        }

        return products;
    }
}