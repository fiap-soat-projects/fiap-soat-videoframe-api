using Business.Entities.Page;
using Domain.Entities;
using Infrastructure.Entities;
using Infrastructure.Entities.Page;

namespace Adapter.Gateways.Extensions;

public static class VideoMongoDbExtensions
{
    extension(VideoMongoDb videoMongoDb)
    {
        public Video ToDomain()
        {
            var video = new Video
            (
                videoMongoDb.Id,
                videoMongoDb.CreatedAt,
                videoMongoDb.UserId!,
                videoMongoDb.Path!,
                videoMongoDb.Name!,
                videoMongoDb.ContentType!,
                videoMongoDb.ContentLength
            );

            return video;
        }
    }

    extension(PagedResult<VideoMongoDb> pagedResult)
    {
        public Pagination<Video> ToDomain()
        {
            return new Pagination<Video>()
            {
                Page = pagedResult.Page,
                Size = pagedResult.Size,
                TotalPages = pagedResult.TotalPages,
                TotalCount = pagedResult.TotalCount,
                Items = pagedResult.Items.Select(item => item.ToDomain())
            };
        }
    }
}
