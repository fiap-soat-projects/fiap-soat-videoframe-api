using Domain.Entities;
using Infrastructure.Repositories.Entities;

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
                videoMongoDb.ContentType!
            );

            return video;
        }
    }
}
