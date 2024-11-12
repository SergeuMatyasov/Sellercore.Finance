using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Sellercore.Finance.WebApi.Middleware.ConditionalAuth;

public class ConditionalAuthMiddleware(RequestDelegate next, IConfiguration config)
{
    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint != null)
        {
            if (HasControllerAuthorizeAttribute(endpoint))
            {
                await PerformAuthorization(context);
            }
        }

        await next(context);
    }

    /// <summary>
    /// Есть ли у endpoint аттрибут [Authorize]?
    /// </summary>
    private bool HasControllerAuthorizeAttribute(Endpoint endpoint)
    {
        var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (controllerActionDescriptor != null)
        {
            var isAuthorizedAccess = controllerActionDescriptor.MethodInfo
                                         .GetCustomAttributes(typeof(AuthorizeAttribute), true).Any()
                                     || controllerActionDescriptor.ControllerTypeInfo
                                         .GetCustomAttributes(typeof(AuthorizeAttribute), true).Any();

            return isAuthorizedAccess;
        }

        return false;
    }
    
    /// <summary>
    /// Выполнить авторизацию (проверку токена) через IdentityService
    /// </summary>
    /// <param name="context"></param>
    private async Task PerformAuthorization(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401; // No token found
            return;
        }

        using var client = new HttpClient();
        client.BaseAddress = new Uri(config["IdentityService:Host"]);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await client.GetAsync($"/tokens/validate?token={token}");
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            context.Response.StatusCode = 403; // Unauthorized
            return;
        }

        if (!response.IsSuccessStatusCode)
        {
            // Ошибка сервиса авторизации
            context.Response.StatusCode = (int)response.StatusCode;
        }
    }
}