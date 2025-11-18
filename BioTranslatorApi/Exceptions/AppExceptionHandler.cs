using Microsoft.AspNetCore.Diagnostics;

namespace BioTranslatorApi.Exceptions;

public class AppExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AppExceptionHandler> _logger;

    public AppExceptionHandler(ILogger<AppExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        (int statusCode, string errorMessage) = exception switch
        {
            BadHttpRequestException badHttpRequest => (403, badHttpRequest.Message),
            _ => (500, "An unexpected error occurred.")
        };

        _logger.LogError(exception, exception.Message);
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new { error = errorMessage }, cancellationToken);

        return true;
    }
}