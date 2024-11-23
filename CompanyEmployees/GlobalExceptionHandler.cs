using System.Net;
using Contracts.Logging;
using Entities.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace CompanyEmployees;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILoggerManager _logger;

    public GlobalExceptionHandler(ILoggerManager logger) =>
        _logger = logger;
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature is not null)
        {
            _logger.LogError($"Something went wrong: { exception.Message }");

            await httpContext.Response.WriteAsync(new ErrorDetailsModel()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "Internal Server Error",
            }.ToString());
        }

        return true;
    }
}