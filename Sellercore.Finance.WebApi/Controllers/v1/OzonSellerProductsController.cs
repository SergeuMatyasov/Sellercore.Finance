using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sellercore.Finance.Ozon.Application.UseCases.Products.Commands.SetOzonSellerProductsCost;
using Sellercore.Finance.Ozon.Application.UseCases.Products.Commands.SyncOzonProducts;

namespace Sellercore.Finance.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class OzonSellerProductsController(ISender sender) : BaseController
{
    /// <summary>
    /// Синхронизировтаь наличие товаров в Ozon и базе данных.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SyncOzonProducts([FromBody] SyncOzonProductsCommand request,
        CancellationToken cancellationToken = default)
    {
        await sender.Send(request, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Уставить себестоимость для товаров Ozon внутри системы.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SetCost([FromBody] SetOzonSellerProductsCostCommand request,
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
        return Ok();
    }

    [HttpPost("test-excel")]
    public async Task<IActionResult> UploadExcel(IFormFile file, [FromForm] string description,
        CancellationToken cancellationToken)
    {
        return Ok();
    }
}