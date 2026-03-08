using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Repositories.Interfaces;

internal interface IDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    string Id { get; set; }

    DateTime CreatedAt { get; }
}
