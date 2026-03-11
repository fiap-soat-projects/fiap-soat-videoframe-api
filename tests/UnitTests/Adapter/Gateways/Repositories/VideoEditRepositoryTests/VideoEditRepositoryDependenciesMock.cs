using Adapter.Gateways.Repositories;
using Domain.Gateways.Repositories.Interfaces;
using Infrastructure.Repositories.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoEditRepositoryTests;

public abstract class VideoEditRepositoryDependenciesMock
{
    protected readonly IVideoEditMongoDbRepository _videoEditMongoDbRepositoryMock;
    protected readonly IVideoEditRepository _sut;

    protected VideoEditRepositoryDependenciesMock()
    {
        _videoEditMongoDbRepositoryMock = Substitute.For<IVideoEditMongoDbRepository>();
        _sut = new VideoEditRepository(_videoEditMongoDbRepositoryMock);
    }
}
