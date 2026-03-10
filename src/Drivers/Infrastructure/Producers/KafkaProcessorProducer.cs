using Confluent.Kafka;
using Infrastructure.Producers.DTOs;
using Infrastructure.Producers.Interfaces;
using Infrastructure.Providers;
using System.Text.Json;

namespace Infrastructure.Producers;

public class KafkaProcessorProducer : IKafkaProcessorProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topicName;

    public KafkaProcessorProducer(IProducer<Null, string> producer)
    {
        _producer = producer;
        _topicName = StaticEnvironmentVariableProvider.KafkaProduceTopicName;
    }

    public async Task ProduceAsync(KafkaProcessorMessage message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var payload = JsonSerializer.Serialize(message);

        var result = await _producer.ProduceAsync(
            _topicName,
            new Message<Null, string> { Value = payload },
            cancellationToken).ConfigureAwait(false);

        if (result.Status == PersistenceStatus.NotPersisted)
        {
            throw new Exception("Message not persisted");
        }
    }
}