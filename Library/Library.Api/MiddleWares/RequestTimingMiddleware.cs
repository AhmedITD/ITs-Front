namespace Library.Api.MiddleWares;

public class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        await next(context);
        stopwatch.Stop();
        
        var elapsed = stopwatch.ElapsedMilliseconds;
        logger.LogInformation("running request: {Method} {Path} took {Elapsed}ms", context.Request.Method, context.Request.Path, elapsed);
    }
}