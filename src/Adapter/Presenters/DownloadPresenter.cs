using Adapter.Presenters.DTOs;

namespace Adapter.Presenters;

public class DownloadPresenter
{
    public DownloadResponse Response { get; init; }

    public DownloadPresenter(string fileName, string contentType, Stream content)
    {
        Response = new DownloadResponse(fileName, contentType, content);
    }
}
