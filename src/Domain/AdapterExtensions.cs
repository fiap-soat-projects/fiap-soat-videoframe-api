using Domain.UseCases;
using Domain.UseCases.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Domain;

[ExcludeFromCodeCoverage]
public static class AdapterExtensions
{
    public static IServiceCollection InjectDomainDependencies(this IServiceCollection services)
    {
        return services.RegisterUseCases();
    }

    private static IServiceCollection RegisterUseCases(this IServiceCollection services)
    {
        return services
         .AddSingleton<IVideoUseCase, VideoUseCase>()
         .AddSingleton<IVideoEditUseCase, VideoEditUseCase>();
    }
}
