using Amazon.CognitoIdentityProvider;
using Amazon.S3;
using Confluent.Kafka;
using Infrastructure.Clients;
using Infrastructure.Clients.Interfaces;
using Infrastructure.Exceptions;
using Infrastructure.MongoDb.Connections;
using Infrastructure.MongoDb.Connections.Interfaces;
using Infrastructure.MongoDb.Factories;
using Infrastructure.MongoDb.Options;
using Infrastructure.Producers;
using Infrastructure.Producers.Interfaces;
using Infrastructure.Providers;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureExtensions
{
    const string APP_NAME_VARIABLE_KEY = "AppName";
    const string DEFAULT_CLUSTER_NAME = "default";

    public static IServiceCollection InjectInfrastructureDependencies(this IServiceCollection services)
    {
        services
            .RegisterMongoDbRepositories()
            .RegisterConnections()
            .RegisterClients()
            .RegisterProducers()
            .RegisterCognitoClient();

        MongoGlobalOptions.Init();

        return services;
    }

    public static IServiceCollection RegisterMongoDbRepositories(this IServiceCollection services)
    {
        services
            .AddSingleton<IVideoMongoDbRepository, VideoMongoDbRepository>()
            .AddSingleton<IVideoEditMongoDbRepository, VideoEditMongoDbRepository>();

        return services;
    }

    public static IServiceCollection RegisterCognitoClient(this IServiceCollection services)
    {
        var cognitoClient = new AmazonCognitoIdentityProviderClient();

        services
            .AddSingleton(cognitoClient)
            .AddSingleton<ICognitoClient, CognitoClient>();

        return services;
    }


    private static IServiceCollection RegisterConnections(this IServiceCollection services)
    {
        var mongoConnectionString = StaticEnvironmentVariableProvider.VideoframeMongoDbConnectionString;
        var appName = Environment.GetEnvironmentVariable(APP_NAME_VARIABLE_KEY);

        EnvironmentVariableNotFoundException.ThrowIfIsNullOrWhiteSpace(appName, APP_NAME_VARIABLE_KEY);

        var connection = new MongoConnection(DEFAULT_CLUSTER_NAME, mongoConnectionString!, appName);

        services
            .AddSingleton<IMongoConnection>(connection)
            .AddSingleton(MongoDataContextFactory.Create);

        return services;
    }

    public static IServiceCollection RegisterClients(this IServiceCollection services)
    {

        var s3url = StaticEnvironmentVariableProvider.S3BucketBaseUrl;
        var s3user = StaticEnvironmentVariableProvider.S3BucketUser;
        var s3password = StaticEnvironmentVariableProvider.S3BucketPassword;
        var s3BucketName = StaticEnvironmentVariableProvider.S3BucketName;
        var config = new AmazonS3Config
        {
            ServiceURL = s3url,
            ForcePathStyle = true
        };

        var amazonS3Client = new AmazonS3Client(s3user, s3password, config);
        var s3BucketClient = new S3BucketClient(amazonS3Client, s3BucketName);

        services.AddSingleton<IS3BucketClient>(s3BucketClient);

        return services;
    }

    public static IServiceCollection RegisterProducers(this IServiceCollection services)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = StaticEnvironmentVariableProvider.KafkaConnectionString
        };

        var producer = new ProducerBuilder<Null, string>(producerConfig).Build();


        services.AddSingleton(producer);
        services.AddSingleton<IKafkaProcessorProducer, KafkaProcessorProducer>();

        return services;
    }
}
