using Infrastructure.MongoDb.Connections.Interfaces;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.MongoDb.Connections;

[ExcludeFromCodeCoverage]
internal class MongoConnection : IMongoConnection
{
    public string? AppName { get; private set; }
    public string ClusterName { get; private set; }
    public MongoUrl MongoUrl { get; private set; }
    public IMongoClient Client { get; private set; }

    public MongoConnection(string clusterName, string connectionString, string? appName = null)
    {
        AppName = appName;
        ClusterName = clusterName;
        MongoUrl = new MongoUrl(connectionString);

        var mongoSettings = MongoClientSettings.FromConnectionString(connectionString);

        if (string.IsNullOrWhiteSpace(appName) is false)
        {
            mongoSettings.ApplicationName = appName;
        }

        Client = new MongoClient(mongoSettings);
    }
}
