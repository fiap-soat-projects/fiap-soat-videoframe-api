using Domain.Producers;
using Domain.Producers.DTOs;
using Infrastructure.Producers.DTOs;
using Infrastructure.Producers.Interfaces;

namespace Adapter.Gateways.Producers;

internal class EditProcessorProducer : IEditProcessorProducer
{
    private readonly IKafkaProcessorProducer _kafkaProcessorProducer;

    public EditProcessorProducer(IKafkaProcessorProducer kafkaProcessorProducer)
    {
        _kafkaProcessorProducer = kafkaProcessorProducer;
    }

    public async Task ProduceAsync(EditProcessorMessage message, CancellationToken cancellationToken)
    {
        var kafkaMessage = new KafkaProcessorMessage(
            message.EditId,
            message.UserName,
            message.UserRecipient,
            message.VideoPath,
            message.EditType.ToString());

        await _kafkaProcessorProducer.ProduceAsync(kafkaMessage, cancellationToken);
    }
}
