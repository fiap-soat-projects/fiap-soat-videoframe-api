using Domain.UseCases;
using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Producers;
using Domain.Gateways.Repositories.Interfaces;
using Domain.UseCases.Interfaces;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoEditUseCaseTests;

public abstract class VideoEditUseCaseDependenciesMock
{
    protected readonly IVideoEditRepository _videoEditRepository;
    protected readonly IBucketClient _bucketClient;
    protected readonly IEditProcessorProducer _editProcessorProducer;

    protected readonly IVideoEditUseCase _sut;

    protected VideoEditUseCaseDependenciesMock()
    {
        _videoEditRepository = Substitute.For<IVideoEditRepository>();
        _bucketClient = Substitute.For<IBucketClient>();
        _editProcessorProducer = Substitute.For<IEditProcessorProducer>();

        _sut = new VideoEditUseCase(
            _videoEditRepository,
            _bucketClient,
            _editProcessorProducer);
    }
}
