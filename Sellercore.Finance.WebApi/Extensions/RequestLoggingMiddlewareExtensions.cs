using Sellercore.Finance.WebApi.Middleware.RequestLogging;

namespace Sellercore.Finance.WebApi.Extensions;

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}