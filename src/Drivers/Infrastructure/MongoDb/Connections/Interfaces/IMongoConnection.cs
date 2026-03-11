using MongoDB.Driver;

namespace Infrastructure.MongoDb.Connections.Interfaces;

internal interface IMongoConnection
{
    public string? AppName { get; }
    public string ClusterName { get; }
    public MongoUrl MongoUrl { get; }
    public IMongoClient Client { get; }
}
