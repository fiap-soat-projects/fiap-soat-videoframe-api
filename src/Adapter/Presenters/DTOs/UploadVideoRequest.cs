namespace Adapter.Presenters.DTOs;

public record UploadVideoRequest(string FileName, Stream Content)
{
}
