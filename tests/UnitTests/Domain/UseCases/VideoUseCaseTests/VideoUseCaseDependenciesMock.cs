using System;
using System.Reflection;
using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Repositories.Interfaces;
using Domain.UseCases;
using Domain.UseCases.Interfaces;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoUseCaseTests;

public abstract class VideoUseCaseDependenciesMock
{
    protected readonly IVideoRepository _videoRepository;
    protected readonly IBucketClient _bucketClient;

    protected readonly IVideoUseCase _sut;

    protected VideoUseCaseDependenciesMock()
    {
        _videoRepository = Substitute.For<IVideoRepository>();
        _bucketClient = Substitute.For<IBucketClient>();
        _sut = new VideoUseCase(_videoRepository, _bucketClient);
    }
}
