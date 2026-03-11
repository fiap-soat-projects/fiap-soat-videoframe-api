using Adapter.Controllers;
using Adapter.Controllers.Interfaces;
using Domain.UseCases.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoEditControllerTests;

public abstract class VideoEditControllerDependenciesMock
{
    protected readonly IVideoEditUseCase _videoEditUseCaseMock;
    protected readonly IVideoUseCase _videoUseCaseMock;
    protected readonly IVideoEditController _sut;

    protected VideoEditControllerDependenciesMock()
    {
        _videoEditUseCaseMock = Substitute.For<IVideoEditUseCase>();
        _videoUseCaseMock = Substitute.For<IVideoUseCase>();
        _sut = new VideoEditController(_videoEditUseCaseMock, _videoUseCaseMock);
    }
}
