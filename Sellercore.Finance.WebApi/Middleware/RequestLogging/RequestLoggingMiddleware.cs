using System.Diagnostics;
using Serilog;
using Shared.Application.Common.Interfaces;

namespace Sellercore.Finance.WebApi.Middleware.RequestLogging;

public class RequestLoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        //var userId = currentUserService.UserId;

        try
        {
            // Логируем начало обработки запроса
            Log.Information("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);

            await next(context);

            stopwatch.Stop();

            // Логируем успешное завершение обработки запроса
            Log.Information("Finished handling request: {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            // Логируем исключение, если оно произошло
            Log.Error(ex, "Error handling request: {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);

            throw; // Не забываем пробрасывать исключение дальше
        }
    }
}