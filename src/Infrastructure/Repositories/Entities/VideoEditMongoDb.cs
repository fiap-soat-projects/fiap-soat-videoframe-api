using Infrastructure.Repositories.Attributes;

namespace Infrastructure.Repositories.Entities;

[BsonCollection("videoEdit")]
public class VideoEditMongoDb : Document
{
    public string? UserId { get; set; }
    public string? Recipient { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
    public string? VideoId { get; set; }
}
