using Domain.Entities.Exceptions;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace Domain.Entities;

public class User
{
    public User(string id, string name, Email email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public string? Id
    {
        get;
        set
        {
            InvalidEntityPropertyException<Video>.ThrowIfNullOrWhiteSpace(value, nameof(Id));
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

    public Email Email { get; set; }
}
