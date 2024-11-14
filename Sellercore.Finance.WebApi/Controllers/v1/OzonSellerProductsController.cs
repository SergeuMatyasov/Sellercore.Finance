using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sellercore.Finance.Ozon.Application.UseCases.Products.Commands.SyncOzonProducts;

namespace Sellercore.Finance.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class OzonSellerProductsController(ISender sender) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> SyncOzonProducts([FromBody] SyncOzonProductsCommand request,
        CancellationToken cancellationToken = default)
    {
        await sender.Send(request, cancellationToken);
        return Ok();
    }
}