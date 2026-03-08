using Adapter.Controllers.Interfaces;
using Adapter.Presenters.DTOs;
using Api.Authentication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

[Authorize]
[ApiController]
[Route("v1/user/videos/edits")]
public class VideoEdit : ControllerBase
{
    private readonly IUserContext _userContext;
    private readonly IVideoEditController _videoEditController;

    public VideoEdit(IUserContext userContext, IVideoEditController editionController)
    {
        _userContext = userContext;
        _videoEditController = editionController;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginatedAsync(
        [FromQuery] PaginationRequest paginationRequest,
        CancellationToken cancellationToken)
    {
        var userRequest = new UserRequest(_userContext.Id, _userContext.Name, _userContext.Email);

        var presenter = await _videoEditController.GetPaginatedAsync(userRequest, paginationRequest, cancellationToken);

        return Ok(presenter.ViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateVideoEditRequest req,
        CancellationToken cancellationToken)
    {
        var userRequest = new UserRequest(_userContext.Id, _userContext.Name, _userContext.Email);

        var id = await _videoEditController.CreateAsync(req, userRequest, cancellationToken);

        return Ok(id);
    }


    [HttpPost]
    [Route("{id}/start")]
    public async Task<IActionResult> StartAsync(
        [FromBody] string id,
        CancellationToken cancellationToken)
    {
        var userRequest = new UserRequest(_userContext.Id, _userContext.Name, _userContext.Email);

        await _videoEditController.StartAsync(id, userRequest, cancellationToken);

        return NoContent();
    }

    [HttpPatch]
    [Route("{id}/status")]
    public async Task<IActionResult> UpdateStatusAsync(
        [FromQuery] string id, 
        [FromBody] UpdateEditionStatusRequest req,
        CancellationToken cancellationToken)
    {
        var userRequest = new UserRequest(_userContext.Id, _userContext.Name, _userContext.Email);

        await _videoEditController.UpdateStatusAsync(id, req.Status, userRequest, cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [Route("{id}/download")]
    public async Task DownloadAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var userRequest = new UserRequest(_userContext.Id, _userContext.Name, _userContext.Email);

        var presenter = await _videoEditController.DownloadAsync(id, userRequest, cancellationToken);

        Response.ContentType = presenter.ViewModel.ContentType;
        Response.Headers.ContentDisposition = $"attachment; filename={presenter.ViewModel.FileName}";

        using (presenter.ViewModel.Content)
        {
            await presenter
             .ViewModel
             .Content
             .CopyToAsync(Response.Body, cancellationToken);
        }
    }
}

