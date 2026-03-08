using Business.Entities.Page;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Producers;
using Domain.Gateways.Producers.DTOs;
using Domain.Gateways.Repositories.Interfaces;
using Domain.UseCases.Interfaces;

namespace Domain.UseCases;

internal class VideoEditUseCase : IVideoEditUseCase
{
    private readonly IVideoEditRepository _videoEditRepository;
    private readonly IBucketClient _bucketClient;
    private readonly IEditProcessorProducer _editProcessorProducer;

    public VideoEditUseCase(
        IVideoEditRepository videoEditRepository,
        IBucketClient bucketClient, 
        IEditProcessorProducer editProcessorProducer)
    {
        _videoEditRepository = videoEditRepository;
        _bucketClient = bucketClient;
        _editProcessorProducer = editProcessorProducer;
    }

    public Task<string> CreateAsync(VideoEdit videoEdit, CancellationToken cancellationToken)
    {
        return _videoEditRepository.InsertOneAsync(videoEdit, cancellationToken);
    }

    public Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken)
    {
        return _bucketClient.DownloadFileAsync(path, cancellationToken);
    }

    public async Task<VideoEdit> GetByIdAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var videoEdit = await _videoEditRepository.GetByIdAsync(id, userId, cancellationToken);

        EntityNotFoundException<VideoEdit>.ThrowIfNull(videoEdit, id);

        return videoEdit!;
    }

    public async Task<string> GetLinkAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var videoEdit = await GetByIdAsync(id, userId, cancellationToken);

        if (videoEdit.Status != EditStatus.Processed)
        {
            throw new Exception("This edit is not processed");
        }

        return await _bucketClient.GetPreSignedDownloadUrlAsync(videoEdit.EditPath!, cancellationToken);
    }

    public async Task<Pagination<VideoEdit>> GetPaginatedAsync(string userId, int page, int size, CancellationToken cancellationToken)
    {
        var edits = await _videoEditRepository.GetPaginatedAsync(userId, page, size, cancellationToken);
        return edits;
    }

    public async Task ProcessAsync(
        Video video,
        VideoEdit videoEdit,
        User user,
        CancellationToken cancellationToken)
    {
        var message = new EditProcessorMessage(
            videoEdit.Id!, 
            user.Id!, 
            user.Name!, 
            user.Email,
            video.Path!,
            videoEdit.Type);

        await _editProcessorProducer.ProduceAsync(message, cancellationToken);
    }

    public async Task UpdateStatusAsync(string id, EditStatus status, string userId, CancellationToken cancellationToken)
    {
        await _videoEditRepository.UpdateStatusAsync(id, userId, status, cancellationToken);
    }
}
