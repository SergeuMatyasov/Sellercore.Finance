using Microsoft.AspNetCore.Mvc;
using Sellercore.Shared.Application.Interfaces;

namespace Sellercore.Finance.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class ExampleController(IYandexKeyManagementService yandexKeyManagementService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Action()
    {
        string data = "my-data";

        string xdata = await yandexKeyManagementService.EncryptAsync(data);
        string ydata = await yandexKeyManagementService.DecryptAsync(xdata);
        
        return Ok(ydata);
    }
}