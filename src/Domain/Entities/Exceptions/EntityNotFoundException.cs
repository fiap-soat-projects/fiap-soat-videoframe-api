using Domain.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Exceptions;

[ExcludeFromCodeCoverage]
public class EntityNotFoundException<TEntity> : DomainException where TEntity : IDomainEntity
{
    private static readonly string _entityClassName = typeof(TEntity).Name;

    private const string ENTITY_NOT_FOUND_TEMPLATE_MESSAGE = "The {0} '{1}' was not found";

    protected EntityNotFoundException(string id)
        : base(string.Format(ENTITY_NOT_FOUND_TEMPLATE_MESSAGE, _entityClassName, id))
    {

    }

    public static void ThrowIfNull(TEntity? entity, string identifier)
    {
        if (entity is null)
        {
            throw new EntityNotFoundException<TEntity>(identifier);
        }
    }
}
