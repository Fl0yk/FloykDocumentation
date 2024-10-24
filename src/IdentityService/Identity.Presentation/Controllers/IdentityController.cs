using AutoMapper;
using Identity.Application.Abstractions.Services;
using Identity.Application.Services.Requests.IdentityRequests;
using Identity.Application.Shared.Models;
using Identity.DataAccess.Constants;
using Identity.Presentation.Shared.Models.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public IdentityController(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    [HttpPost("registration")]
    public async Task<IActionResult> RegistrationPost([FromBody]RegistrationUserRequestDTO request, CancellationToken cancellationToken)
    {
        AccessToken result =  await _identityService.RegistrationAsync(
            _mapper.Map<RegistrationUserRequest>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginPost([FromBody]LoginUserRequestDTO request, CancellationToken cancellationToken)
    {
        AccessToken result = await _identityService.LoginAsync(
            _mapper.Map<LoginUserRequest>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshPost([FromBody] RefreshTokenRequestDTO request, CancellationToken cancellationToken)
    {
        AccessToken result = await _identityService.ReefreshAsync(
            _mapper.Map<RefreshTokenRequest>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("add-to-role")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AddUserToRolePost([FromBody] AddUserToRoleRequestDTO request, CancellationToken cancellationToken)
    {
        await _identityService.AddUserToRoleAsync(
            _mapper.Map<AddUserToRoleRequest>(request), 
            cancellationToken);

        return NoContent();
    }
}
