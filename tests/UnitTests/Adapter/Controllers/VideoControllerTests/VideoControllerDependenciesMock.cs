using Adapter.Controllers;
using Adapter.Controllers.Interfaces;
using Domain.UseCases.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoControllerTests;

public abstract class VideoControllerDependenciesMock
{
    protected readonly IVideoUseCase _videoUseCaseMock;
    protected readonly IVideoController _sut;

    protected VideoControllerDependenciesMock()
    {
        _videoUseCaseMock = Substitute.For<IVideoUseCase>();
        _sut = new VideoController(_videoUseCaseMock);
    }
}
