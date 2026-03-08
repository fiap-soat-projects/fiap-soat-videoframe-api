using Domain.Entities;
using Domain.Producers.DTOs;

namespace Domain.Producers;

public interface IEditProcessorProducer
{
    Task ProduceAsync(EditProcessorMessage videoEdit, CancellationToken cancellationToken);
}
