using Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities;


[BsonIgnoreExtraElements]
[BsonDiscriminator("video")]
public class VideoMongoDb : Document
{
    public string? UserId { get; set; }
    public string? Path { get; set; }
    public string? Name { get; set; }
    public string? ContentType { get; set; }

    public VideoMongoDb(Video video)
    {
        UserId = video.UserId;
        Path = video.Path;
        Name = video.Name;
        ContentType = video.ContentType;
    }
}
