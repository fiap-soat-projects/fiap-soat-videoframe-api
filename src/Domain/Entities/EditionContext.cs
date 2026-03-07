namespace Domain.Entities;

public class EditionContext
{
    public string? Id { get; set; }
    public string UserId { get; set; }
    public string Recipient { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string VideoId { get; set; }

    public EditionContext(string? id, string userId, string recipient, string type, string status, string videoId)
    {
        Id = id;
        UserId = userId;
        Recipient = recipient;
        Type = type;
        Status = status;
        VideoId = videoId;
    }

    public EditionContext(string userId, string recipient, string type, string status, string videoId)
    {
        UserId = userId;
        Recipient = recipient;
        Type = type;
        Status = status;
        VideoId = videoId;
    }
}
