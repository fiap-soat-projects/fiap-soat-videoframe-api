namespace Api.Middlewares.Exceptions;

public class UploadContentTypeException : Exception
{
    private const string DEFAULT_MESSAGE = "This content-type is not allowed, only mp4";

    public UploadContentTypeException() : base(DEFAULT_MESSAGE)
    {
        
    }
}
