using Adapter.Gateways.Producers;
using Domain.Gateways.Producers;
using Infrastructure.Producers.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Producers.EditProcessorProducerTests;

public abstract class EditProcessorProducerDependenciesMock
{
    protected readonly IKafkaProcessorProducer _kafkaProcessorProducerMock;
    protected readonly IEditProcessorProducer _sut;

    protected EditProcessorProducerDependenciesMock()
    {
        _kafkaProcessorProducerMock = Substitute.For<IKafkaProcessorProducer>();
        _sut = new EditProcessorProducer(_kafkaProcessorProducerMock);
    }
}
