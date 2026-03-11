using Domain.Entities;
using Domain.Gateways.Producers.DTOs;

namespace Domain.Gateways.Producers;

public interface IEditProcessorProducer
{
    Task ProduceAsync(EditProcessorMessage videoEdit, CancellationToken cancellationToken);
}
