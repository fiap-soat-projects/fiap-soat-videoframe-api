using Amazon.Runtime.Internal;
using Api.Middlewares.Exceptions;
using Api.Middlewares.Models;
using FileSignatures;
using FileSignatures.Formats;
using System.Buffers;

public class FileTypeValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly FileFormatInspector _inspector;

    public FileTypeValidationMiddleware(RequestDelegate next)
    {
        _next = next;

        _inspector = new FileFormatInspector([new MP4()]);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var isUploadRequest =
            context.Request.Path == "/v1/user/videos" &&
            context.Request.Method == "POST";

        if (!isUploadRequest)
        {
            await _next(context);
            return;
        }
        
        if(context.Request.ContentType != "video/mp4")
        {
            throw new UploadContentTypeException();
        }

        var reader = context.Request.BodyReader;

        var result = await reader.ReadAsync();
        var buffer = result.Buffer;

        var header = buffer.Slice(0, Math.Min(32, buffer.Length));
        var headerBytes = header.ToArray();

        var format = _inspector.DetermineFileFormat(new MemoryStream(headerBytes));

        if (format == null)
        {
            throw new UploadContentTypeException();
        }

        reader.AdvanceTo(buffer.Start);

        await _next(context);
    }
}