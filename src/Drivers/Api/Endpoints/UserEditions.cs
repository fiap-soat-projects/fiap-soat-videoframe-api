using Adapter.Presenters.DTOs;
using Api.Authentication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

[ApiController]
[Route("v1/user/editions")]
public class UserEditions : ControllerBase
{
    private readonly IUserContext _userContext;

    public UserEditions(IUserContext userContext)
    {
        _userContext = userContext;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateEditionRequest req,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpPatch]
    [Route("{id}/status")]
    public async Task<IActionResult> UpdateStatusAsync(
        [FromQuery] string id, 
        [FromBody] UpdateEditionStatusRequest req,
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet]
    [Route("{id}/download")]
    public async Task<IActionResult> DownloadAsync([FromRoute] string id)
    {
        return Ok();
    }
}

