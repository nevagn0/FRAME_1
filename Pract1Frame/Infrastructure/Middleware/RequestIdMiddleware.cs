namespace Pract1Frame.Middleware;

public class RequestIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestIdMiddleware> _logger;

    public RequestIdMiddleware(RequestDelegate next, ILogger<RequestIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;
        
        context.Response.OnStarting(() =>
        {
            if (!context.Response.Headers.ContainsKey("X-Request-ID"))
            {
                context.Response.Headers.Append("X-Request-ID", requestId);
            }
            return Task.CompletedTask;
        });

        _logger.LogInformation("[{RequestId}] Вход в контроллер: {Method} {Path}", 
            requestId, context.Request.Method, context.Request.Path);

        try
        {
            await _next(context);
        }
        finally
        {
            _logger.LogInformation("[{RequestId}]  Выход из контроллера: {StatusCode}", 
                requestId, context.Response.StatusCode);
        }
    }
}