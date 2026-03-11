using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Page;
using Infrastructure.Entities;
using Infrastructure.Entities.Page;

namespace Adapter.Gateways.Extensions;

public static class VideoEditMongoDbExtensions
{
    extension(VideoEditMongoDb videoEditMongoDb)
    {
        public VideoEdit ToDomain()
        {

            var isEditTypeValid = Enum.TryParse<EditType>(videoEditMongoDb.Type, out var editType);
            var isEditStatusValid = Enum.TryParse<EditStatus>(videoEditMongoDb.Status, out var editStatus);

            if ((isEditTypeValid || isEditStatusValid) is false)
            {
                throw new Exception("Error while trying to parse videoEdit mongoDb entity");
            }

            var videoEdit = new VideoEdit
            (
                videoEditMongoDb.Id,
                videoEditMongoDb.CreatedAt,
                videoEditMongoDb.UserId!,
                videoEditMongoDb.Recipient!,
                editType,
                editStatus,
                videoEditMongoDb.VideoId!,
                videoEditMongoDb.EditPath!,
                videoEditMongoDb.NotificationTargets.Select(x => x.ToDomain())
            );

            return videoEdit;
        }
    }

    extension(PagedResult<VideoEditMongoDb> pagedResult)
    {
        public Pagination<VideoEdit> ToDomain()
        {
            return new Pagination<VideoEdit>
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
