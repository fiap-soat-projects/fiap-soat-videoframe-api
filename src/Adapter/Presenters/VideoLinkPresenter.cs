namespace Adapter.Presenters;

public class VideoLinkPresenter
{
    public string Link { get; init; }

    public VideoLinkPresenter(string link)
    {
        Link = link;
    }
}
