using Confluent.Kafka;
using Infrastructure.Producers.DTOs;
using Infrastructure.Producers.Interfaces;
using System.Text.Json;

namespace Infrastructure.Producers;

public class KafkaProcessorProducer : IKafkaProcessorProducer
{
    private readonly IProducer<Null, string> _producer;
    private const string Topic = "video-processor";

    public KafkaProcessorProducer(IProducer<Null, string> producer)
    {
        _producer = producer;
    }

    public async Task ProduceAsync(KafkaProcessorMessage message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var payload = JsonSerializer.Serialize(message);

        var result = await _producer.ProduceAsync(
            Topic,
            new Message<Null, string> { Value = payload },
            cancellationToken).ConfigureAwait(false);

        if (result.Status == PersistenceStatus.NotPersisted)
        {
            throw new Exception("Message not persisted");
        }
    }
}