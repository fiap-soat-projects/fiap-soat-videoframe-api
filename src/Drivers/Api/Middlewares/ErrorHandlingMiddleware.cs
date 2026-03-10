using Api.Middlewares.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace Api.Middlewares;

[ExcludeFromCodeCoverage]
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UploadContentTypeException ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);

            await HandleResponseAsync(context, ex, HttpStatusCode.UnsupportedMediaType);
        }
        catch (Exception ex)
        { 
            _logger.LogError(ex, "{Message}", ex.Message);

            await HandleResponseAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private static async Task HandleResponseAsync(
        HttpContext context,
        Exception ex,
        HttpStatusCode statusCode,
        string? replacementMessage = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = context.Response.StatusCode,
            error = string.IsNullOrWhiteSpace(replacementMessage) ? ex.Message : replacementMessage
#if DEBUG
            ,
            details = ex.StackTrace
#endif
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonSerializerOptions));
    }
}