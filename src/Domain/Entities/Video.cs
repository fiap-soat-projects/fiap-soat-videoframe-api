namespace Domain.Entities;

public class Video
{
    public string? Id { get; set; }
    public string UserId { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }

    public Video(string userId, string path, string name, string contentType)
    {
        UserId = userId;
        Path = path;
        Name = name;
        ContentType = contentType;
    }

    public Video(string id, string userId, string path, string name, string contentType)
    {
        Id = id;
        UserId = userId;
        Path = path;
        Name = name;
        ContentType = contentType;
    }
}
