using Identity.Application.Abstractions.Services;
using Identity.Application.Shared.Models;
using Identity.Application.Shared.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("registration")]
    public async Task<IActionResult> RegistrationPost([FromBody]RegistrationUserRequest request, CancellationToken cancellationToken)
    {
        await _identityService.RegistrationAsync(request, cancellationToken);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginPost([FromBody]LoginUserRequest request, CancellationToken cancellationToken)
    {
        AccessToken result = await _identityService.LoginAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshPost([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        AccessToken result = await _identityService.ReefreshAsync(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost("{username}/add-to-role/{roleName}")]
    public async Task<IActionResult> AddUserToRolePost([FromRoute]string username, [FromRoute]string roleName, CancellationToken cancellationToken)
    {
        await _identityService.AddUserToRoleAsync(new(username, roleName), cancellationToken);

        return NoContent();
    }
}
