using Adapter.Presenters.DTOs;
using Api.Authentication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

[ApiController]
[Route("v1/user/videos")]
public class UserVideos : ControllerBase
{
    private readonly IUserContext _userContext;

    public UserVideos(IUserContext userContext)
    {
        _userContext = userContext;
    }


    [HttpPost]
    public async Task<IActionResult> UploadAsync([FromQuery] string fileName)
    {
        return Ok();
    }


    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetLinkAsync([FromQuery] string fileName)
    {
        return Ok();
    }

    [HttpGet]
    [Route("{id}/download")]
    public async Task<IActionResult> DownloadAsync([FromRoute] string id)
    {
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync()
    {
        return Ok();
    }
}
