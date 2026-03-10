using Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Entities;

[BsonIgnoreExtraElements]
[BsonDiscriminator("videoEdit")]
public class VideoEditMongoDb : Document
{
    public string? UserId { get; set; }
    public string? Recipient { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
    public string? VideoId { get; set; }
    public string? EditPath { get; set; }

    public VideoEditMongoDb(VideoEdit videoEdit)
    {
        UserId = videoEdit.UserId;
        Recipient = videoEdit.Recipient;
        Type = videoEdit.Type.ToString();
        Status = videoEdit.Status.ToString();
        VideoId = videoEdit.VideoId;
        EditPath = videoEdit.EditPath;
    }
}
