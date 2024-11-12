using Microsoft.AspNetCore.Mvc;

namespace Sellercore.Finance.WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class BaseController : ControllerBase
{
    
}