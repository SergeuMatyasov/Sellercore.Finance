namespace Sellercore.Finance.WebApi.Middleware.ConditionalAuth;

public static class ConditionalAuthMiddlewareExtensions
{
    public static IApplicationBuilder UseConditionalAuth(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ConditionalAuthMiddleware>();
    }
}