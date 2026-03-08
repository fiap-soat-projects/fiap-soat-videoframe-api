using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.Entities.Interfaces;

namespace Domain.Entities;

public class VideoEdit : IDomainEntity
{
    public string? Id
    {
        get;
        set
        {
            InvalidEntityPropertyException<VideoEdit>.ThrowIfNullOrWhiteSpace(value, nameof(Id));
            field = value;
        }
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? UserId
    {
        get;
        set
        {
            InvalidEntityPropertyException<VideoEdit>.ThrowIfNullOrWhiteSpace(value, nameof(UserId));
            field = value;
        }
    }

    public string? Recipient
    {
        get;
        set
        {
            InvalidEntityPropertyException<VideoEdit>.ThrowIfNullOrWhiteSpace(value, nameof(Recipient));
            field = value;
        }
    }

    public EditType Type { get; set; }
    public EditStatus Status { get; set; }

    public string? VideoId
    {
        get;
        set
        {
            InvalidEntityPropertyException<VideoEdit>.ThrowIfNullOrWhiteSpace(value, nameof(VideoId));
            field = value;
        }
    }

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
