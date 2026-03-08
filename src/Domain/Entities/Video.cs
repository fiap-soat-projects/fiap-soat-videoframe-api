using Domain.Entities.Exceptions;
using Domain.Entities.Interfaces;

namespace Domain.Entities;

public class Video : IDomainEntity
{
    public string? Id 
    { 
        get;
        set 
        {
            InvalidEntityPropertyException<Video>.ThrowIfNullOrWhiteSpace(value, nameof(Id));
            field = value;
        } 
    }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? UserId
    {
        get;
        set
        {
            InvalidEntityPropertyException<Video>.ThrowIfNullOrWhiteSpace(value, nameof(UserId));
            field = value;
        }
    }
    public string? Path
    {
        get;
        set
        {
            InvalidEntityPropertyException<Video>.ThrowIfNullOrWhiteSpace(value, nameof(Path));
            field = value;
        }
    }
    public string? Name
    {
        get;
        set
        {
            InvalidEntityPropertyException<Video>.ThrowIfNullOrWhiteSpace(value, nameof(Name));
            field = value;
        }
    }
    public string? ContentType
    {
        get;
        set
        {
            InvalidEntityPropertyException<Video>.ThrowIfNullOrWhiteSpace(value, nameof(ContentType));
            field = value;
        }
    }

    public Video(string userId, string path, string name, string contentType)
    {
        UserId = userId;
        Path = path;
        Name = name;
        ContentType = contentType;
    }

    public Video(string id, DateTime createdAt, string userId, string path, string name, string contentType)
    {
        Id = id;
        CreatedAt = createdAt;
        UserId = userId;
        Path = path;
        Name = name;
        ContentType = contentType;
    }
}
