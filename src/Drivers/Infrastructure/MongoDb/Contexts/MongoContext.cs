using Infrastructure.MongoDb.Contexts.Interfaces;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.MongoDb.Contexts;

[ExcludeFromCodeCoverage]
internal class MongoContext : IMongoContext
{
    public string ClusterName { get; }
    public IMongoDatabase Database { get; }

    public MongoContext(string clusterName, IMongoDatabase database)
    {
        ClusterName = clusterName;
        Database = database;
    }

    public IMongoCollection<TEntity> GetCollection<TEntity>()
    {
        var collectionName = GetCollectionName<TEntity>();

        return GetCollection<TEntity>(collectionName);
    }

    public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
    {
        return Database.GetCollection<TEntity>(collectionName);
    }

    private static string GetCollectionName<TEntity>()
    {
        var containsDiscriminatorAttribute = Attribute.IsDefined(
            typeof(TEntity),
            typeof(BsonDiscriminatorAttribute));

        if (containsDiscriminatorAttribute)
        {
            var classMap = BsonClassMap.LookupClassMap(typeof(TEntity));

            if (!string.IsNullOrWhiteSpace(classMap.Discriminator))
            {
                return classMap.Discriminator;
            }
        }

        string entityName = typeof(TEntity).Name;

        entityName = char.ToLower(entityName[0]) + entityName[1..];

        return entityName;
    }
}
