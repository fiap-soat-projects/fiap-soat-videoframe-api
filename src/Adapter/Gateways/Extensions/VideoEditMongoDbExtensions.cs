using Domain.Entities;
using Domain.Entities.Enums;
using Infrastructure.Repositories.Entities;

namespace Adapter.Gateways.Extensions;

public static class VideoEditMongoDbExtensions
{
    extension(VideoEditMongoDb videoEditMongoDb)
    {
        public VideoEdit ToDomain()
        {

            var isEditTypeValid = Enum.TryParse<EditType>(videoEditMongoDb.Type, out var editType);
            var isEditStatusValid = Enum.TryParse<EditStatus>(videoEditMongoDb.Type, out var editStatus);

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
                videoEditMongoDb.VideoId!
            );

            return videoEdit;
        }

    }
}
