using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Api.ExceptionHandling;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");

        await httpContext.Response
            .WriteAsJsonAsync(
                Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error"),
                cancellationToken);

        return true;
    }
}
