using Banking.Accounts.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text.Json;

namespace Banking.Accounts.Service.Middlewares;

public sealed class GlobalExceptionMiddleware
{
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(logger);

        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AccountConflictException ex)
        {
            _logger.LogWarning(ex, "Конфликт параллелизма для счета: {AccountId}", ex.AccountId);

            await HandleConflictAsync(context, ex);
        }
        catch (AccountDomainException ex) 
        {
            _logger.LogWarning(ex, ex.Message);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Ошибка операции",
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static async Task HandleConflictAsync(HttpContext context, AccountConflictException ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status409Conflict;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Conflict in account data",
            Detail = ex.Message,
            Instance = context.Request.Path,
            Extensions =
            {
                ["accountId"] = ex.AccountId,
                ["errorCode"] = "CONCURRENCY_CONFLICT"
            }
        };

        var json = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(json);
    }

    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
}