using Adapter.Presenters.DTOs;

namespace Adapter.Presenters;

public class VideoLinkPresenter
{
    public DownloadLinkResponse ViewModel { get; init; }

    public VideoLinkPresenter(string link)
    {
        ViewModel = new DownloadLinkResponse(link);
    }
}
