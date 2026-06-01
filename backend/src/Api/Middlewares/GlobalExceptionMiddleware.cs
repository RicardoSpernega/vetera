using Api.Contracts;
using Application.Common.Exceptions;

namespace Api.Middlewares;

public sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AppException exception)
        {
            logger.LogWarning(exception, "Application error: {Message}", exception.Message);
            context.Response.StatusCode = exception.StatusCode;
            await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail(exception.Message));
        }
        catch (OperationCanceledException)
        {
            context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
            await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail("Request was canceled."));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unexpected error.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail("Unexpected server error."));
        }
    }
}
