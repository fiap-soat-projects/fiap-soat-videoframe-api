using Adapter.Presenters.DTOs;

namespace Adapter.Presenters;

public class DownloadPresenter
{
    public DownloadResponse ViewModel { get; init; }

    public DownloadPresenter(string fileName, string contentType, Stream content)
    {
        ViewModel = new DownloadResponse(fileName, contentType, content);
    }
}
