using Microsoft.AspNetCore.Mvc;

namespace Sellercore.Finance.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class ExampleController : BaseController
{
    [HttpGet]
    public IActionResult Action()
    {
        return Ok();
    }
}