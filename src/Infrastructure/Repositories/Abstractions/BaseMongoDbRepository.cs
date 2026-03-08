using Infrastructure.Repositories.Attributes;
using Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Abstractions;

internal abstract class BaseMongoDbRepository<TDocument> where TDocument : IDocument
{
    protected readonly IMongoCollection<TDocument> _mongoCollection;

    protected BaseMongoDbRepository(IMongoDatabase mongoDatabase)
    {
        var collectionName = GetCollectionName(typeof(TDocument));
        _mongoCollection = mongoDatabase.GetCollection<TDocument>(collectionName);
    }

    protected static string GetCollectionName(Type documentType)
    {
        return (documentType
            .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
            .FirstOrDefault() as BsonCollectionAttribute)?
            .CollectionName!;
    }
}
