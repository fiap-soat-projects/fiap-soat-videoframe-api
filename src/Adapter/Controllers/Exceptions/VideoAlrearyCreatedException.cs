namespace Adapter.Controllers.Exceptions;

public class VideoAlrearyCreatedException : Exception
{
    private const string DEFAULT_MESSAGE = "An video with the same name is alreary created";

    public VideoAlrearyCreatedException() : base(DEFAULT_MESSAGE)
    {
        
    }
}
