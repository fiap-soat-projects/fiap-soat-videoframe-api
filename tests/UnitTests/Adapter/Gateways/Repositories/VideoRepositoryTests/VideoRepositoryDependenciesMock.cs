using Adapter.Gateways.Repositories;
using Domain.Gateways.Repositories.Interfaces;
using Infrastructure.Repositories.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoRepositoryTests;

public abstract class VideoRepositoryDependenciesMock
{
    protected readonly IVideoMongoDbRepository _videoMongoDbRepositoryMock;
    protected readonly IVideoRepository _sut;

    protected VideoRepositoryDependenciesMock()
    {
        _videoMongoDbRepositoryMock = Substitute.For<IVideoMongoDbRepository>();
        _sut = new VideoRepository(_videoMongoDbRepositoryMock);
    }
}
