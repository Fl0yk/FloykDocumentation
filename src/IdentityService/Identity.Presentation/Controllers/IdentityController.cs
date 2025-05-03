using AutoMapper;
using Core.Constants;
using Identity.Application.Shared.Models;
using Identity.Application.UseCases.Command.Identity;
using Identity.Presentation.Shared.Models.DTOs.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public IdentityController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody]RegistrationUserRequestDTO request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<RegisterCommand>(request), cancellationToken);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginUserRequestDTO request, CancellationToken cancellationToken)
    {
        AccessToken result = await _mediator.Send(_mapper.Map<LoginCommand>(request), cancellationToken);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshPost([FromBody] RefreshTokenRequestDTO request, CancellationToken cancellationToken)
    {
        AccessToken result = await _mediator.Send(_mapper.Map<RefreshTokenCommand>(request), cancellationToken);

        return Ok(result);
    }

    [HttpPost("add-to-role")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AddUserToRolePost([FromBody] AddUserToRoleRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(_mapper.Map<AddUserToRoleCommand>(request), cancellationToken);

        return NoContent();
    }
}
