using Adapter.Gateways.Clients;
using Domain.Gateways.Clients.Interfaces;
using Infrastructure.Clients.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.BucketClientTests;

public abstract class BucketClientDependenciesMock
{
    protected readonly IS3BucketClient _s3BucketClientMock;
    protected readonly IBucketClient _sut;

    protected BucketClientDependenciesMock()
    {
        _s3BucketClientMock = Substitute.For<IS3BucketClient>();
        _sut = new BucketClient(_s3BucketClientMock);
    }
}
