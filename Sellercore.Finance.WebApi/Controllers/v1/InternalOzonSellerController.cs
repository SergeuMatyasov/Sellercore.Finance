using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sellercore.Finance.Ozon.Application.UseCases.InternalOzonSeller.Commands.AddInternalOzonSeller;

namespace Sellercore.Finance.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class InternalOzonSellerController(ISender sender) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> AddEncrypted([FromBody] AddInternalOzonSellerCommand request,
        CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
        return Ok();
    }
}