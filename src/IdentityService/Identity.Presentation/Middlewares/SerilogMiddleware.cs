using Serilog;
using System.Diagnostics;

namespace Identity.Presentation.Middlewares;

public class SerilogMiddleware
{
    private readonly RequestDelegate _next;

    private const string _messageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

    public SerilogMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException("Request delegate is null");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch sw = Stopwatch.StartNew();

        try
        {
            await _next(context);

            if (context.Response.StatusCode >= 400 && context.Response.StatusCode < 500)
                WarningHandle(context.Request, context.Response, sw);
        }
        catch (Exception ex)
        {
            ErrorHandle(context.Request, context.Response, sw, ex);
        }
    }

    private static void WarningHandle(HttpRequest request, HttpResponse response, Stopwatch sw)
    {
        sw.Stop();

        var requestMethod = request.Method;
        var requestPath = request.Path;
        var statuseCode = response.StatusCode;

        Log.Warning(_messageTemplate, requestMethod, requestPath, statuseCode, sw.Elapsed.TotalMilliseconds);
    }

    private static void ErrorHandle(HttpRequest request, HttpResponse response, Stopwatch sw, Exception ex)
    {
        sw.Stop();

        var requestMethod = request.Method;
        var requestPath = request.Path;
        var statuseCode = response.StatusCode;

        Log.Error(ex, _messageTemplate, requestMethod, requestPath, statuseCode, sw.Elapsed.TotalMilliseconds);

    }
}
