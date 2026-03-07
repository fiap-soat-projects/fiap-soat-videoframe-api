using Domain.Entities.Enums;

namespace Domain.Entities;

public class VideoEdit
{
    public string? Id { get; set; }
    public string UserId { get; set; }
    public string Recipient { get; set; }
    public EditType Type { get; set; }
    public EditStatus Status { get; set; }
    public string VideoId { get; set; }

    public VideoEdit(string? id, string userId, string recipient, EditType type, EditStatus status, string videoId)
    {
        Id = id;
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
