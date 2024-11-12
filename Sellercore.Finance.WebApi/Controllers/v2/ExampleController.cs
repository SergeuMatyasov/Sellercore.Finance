using Microsoft.AspNetCore.Mvc;

namespace Sellercore.Finance.WebApi.Controllers.v2;

[ApiVersion("2.0")]
public class ExampleController : BaseController
{
    [HttpGet]
    public IActionResult Action()
    {
        return Ok();
    }
}