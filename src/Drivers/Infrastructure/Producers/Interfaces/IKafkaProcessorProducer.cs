using Infrastructure.Producers.DTOs;

namespace Infrastructure.Producers.Interfaces;

public interface IKafkaProcessorProducer
{
    Task ProduceAsync(KafkaProcessorMessage message, CancellationToken cancellationToken);
}
