using Adapter.Controllers;
using Adapter.Controllers.Interfaces;
using Adapter.Gateways.Clients;
using Adapter.Gateways.Producers;
using Adapter.Gateways.Repositories;
using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Producers;
using Domain.Gateways.Repositories.Interfaces;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Adapter;

[ExcludeFromCodeCoverage]
public static class AdapterExtensions
{
    public static IServiceCollection InjectAdapterDependencies(this IServiceCollection services)
    {
        return services
            .RegisterControllers()
            .RegisterGateways()
            .RegisterProducers();
    }

    private static IServiceCollection RegisterControllers(this IServiceCollection services)
    {
        return services
             .AddSingleton<IVideoController, VideoController>()
             .AddSingleton<IVideoEditController, VideoEditController>();
    }

    private static IServiceCollection RegisterProducers(this IServiceCollection services)
    {
        return services.AddSingleton<IEditProcessorProducer, EditProcessorProducer>();
    }

    public static IServiceCollection RegisterGateways(this IServiceCollection services)
    {
        services
            .AddSingleton<IBucketClient, BucketClient>()
            .AddSingleton<IEditProcessorProducer, EditProcessorProducer>()
            .AddSingleton<IVideoRepository, VideoRepository>()
            .AddSingleton<IVideoEditRepository, VideoEditRepository>();

        return services;
    }
}
