using Adapter.Gateways.Producers;
using Infrastructure.Producers.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Producers.EditProcessorProducerTests.Constructor;

public class EditProcessorProducerConstructorTests
{
    [Fact]
    public void When_Valid_IKafkaProcessorProducer_Is_Provided_Then_Construction_Succeeds()
    {
        // Arrange
        var kafkaProcessorProducerMock = Substitute.For<IKafkaProcessorProducer>();

        // Act
        var producer = new EditProcessorProducer(kafkaProcessorProducerMock);

        // Assert
        producer.Should().NotBeNull();
    }
}
