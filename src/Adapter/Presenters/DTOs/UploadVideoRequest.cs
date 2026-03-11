namespace Adapter.Presenters.DTOs;

public record UploadVideoRequest(string FileName, string ContentType, long ContentLength, Stream Content)
{
}
