using Domain.Entities.Enums;

namespace Domain.Entities;

public class VideoEdit
{
    public string? Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; }
    public string Recipient { get; set; }
    public EditType Type { get; set; }
    public EditStatus Status { get; set; }
    public string VideoId { get; set; }

    public VideoEdit(string? id, DateTime createdAt, string userId, string recipient, EditType type, EditStatus status, string videoId)
    {
        Id = id;
        CreatedAt = createdAt;
        UserId = userId;
        Recipient = recipient;
        Type = type;
        Status = status;
        VideoId = videoId;
    }

    public VideoEdit(string userId, string recipient, EditType type, EditStatus status, string videoId)
    {
        UserId = userId;
        Recipient = recipient;
        Type = type;
        Status = status;
        VideoId = videoId;
    }
}
