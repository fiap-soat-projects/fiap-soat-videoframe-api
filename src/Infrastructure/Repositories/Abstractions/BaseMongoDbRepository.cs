using Infrastructure.Entities.Interfaces;
using Infrastructure.Entities.Page;
using Infrastructure.MongoDb.Contexts.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Abstractions;

internal abstract class BaseMongoDbRepository<TDocument> where TDocument : IDocument
{
    protected readonly IMongoCollection<TDocument> _mongoCollection;

    protected BaseMongoDbRepository(IMongoContext mongoContext)
    {
        _mongoCollection = mongoContext.GetCollection<TDocument>();
    }

    protected async Task<PagedResult<TDocument>> GetPagedAsync(
        int page,
        int size,
        FilterDefinition<TDocument> filter,
        SortDefinition<TDocument>? sort = null,
        CancellationToken cancellationToken = default)
    {
        var options = new FindOptions<TDocument>
        {
            Skip = (page - 1) * size,
            Limit = size,
            Sort = sort
        };

        var cursor = await _mongoCollection.FindAsync(
            filter,
            options,
            cancellationToken);

        var orders = cursor.ToEnumerable(cancellationToken: cancellationToken);

        var count = await _mongoCollection.CountDocumentsAsync(
            filter,
            cancellationToken: cancellationToken);

        var pages = size == 0
            ? 0
            : (int)Math.Ceiling(count / (double)size);

        var pageResult = new PagedResult<TDocument>
        {
            Items = orders,
            Page = page,
            Size = size,
            TotalCount = count,
            TotalPages = pages
        };

        return pageResult;
    }
}
