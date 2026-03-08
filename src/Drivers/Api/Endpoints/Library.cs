using Adapter.Controllers.Interfaces;
using Adapter.Presenters.DTOs;
using Api.Authentication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

[Authorize]
[ApiController]
[Route("v1/user/videos")]
public class Library : ControllerBase
{
    private readonly IUserContext _userContext;
    private readonly IVideoController _videoController;

    public Library(IUserContext userContext, IVideoController videoController)
    {
        _userContext = userContext;
        _videoController = videoController;
    }

    [HttpPost]
    public async Task<IActionResult> UploadAsync(
        [FromHeader] string fileName,
        CancellationToken cancellationToken)
    {
        var req = new UploadVideoRequest(fileName, Request.Body);

        var userRequest = new UserRequest(_userContext.Id, _userContext.Name, _userContext.Email);

        var presenter = await _videoController.UploadAsync(req, userRequest, cancellationToken);

        return Ok(presenter.Id);
    }

    [HttpGet]
    [Route("{id}/link")]
    public async Task<IActionResult> GetLinkAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var userRequest = new UserRequest(_userContext.Id, _userContext.Name, _userContext.Email);

        var presenter = await _videoController.GetLinkAsync(id, userRequest, cancellationToken);

        return Ok(presenter);
    }

    [HttpGet]
    [Route("{id}/download")]
    public async Task DownloadAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var userRequest = new UserRequest(_userContext.Id, _userContext.Name, _userContext.Email);

        var presenter = await _videoController.DownloadAsync(id, userRequest, cancellationToken);

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

    [HttpGet]
    public async Task<IActionResult> GetPaginatedAsync(
        [FromQuery] PaginationRequest paginationRequest,
        CancellationToken cancellationToken)
    {
        var userRequest = new UserRequest(_userContext.Id, _userContext.Name, _userContext.Email);

        var presenter = await _videoController.GetPaginatedAsync(userRequest, paginationRequest, cancellationToken);

        return Ok(presenter.ViewModel);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var userRequest = new UserRequest(_userContext.Id, _userContext.Name, _userContext.Email);

        var presenter = await _videoController.GetByIdAsync(id, userRequest, cancellationToken);

        return Ok(presenter.ViewModel);
    }
}
