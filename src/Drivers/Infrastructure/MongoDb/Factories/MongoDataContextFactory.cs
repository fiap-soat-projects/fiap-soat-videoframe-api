using Infrastructure.MongoDb.Connections.Interfaces;
using Infrastructure.MongoDb.Contexts;
using Infrastructure.MongoDb.Contexts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Infrastructure.MongoDb.Factories;

internal class MongoDataContextFactory
{
    const string APP_NAME_ENV_VAR_KEY = "AppName";
    const string DEFAULT_CLUSTER_NAME = "default";

    public static IMongoContext Create(IServiceProvider serviceProvider)
    {
        var appName = Environment.GetEnvironmentVariable(APP_NAME_ENV_VAR_KEY) ?? DEFAULT_CLUSTER_NAME;

        var mongoConnection = serviceProvider
            .GetServices<IMongoConnection>()
            .First(connection => connection.AppName == appName);

        var mongoDatabase = mongoConnection
            .Client
            .GetDatabase(mongoConnection.MongoUrl.DatabaseName);

        return new MongoContext(appName, mongoDatabase);
    }
}
