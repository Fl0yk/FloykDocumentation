using AutoMapper;
using Identity.Application.Shared.Models.DTOs;
using Identity.Application.UseCases.Command.Users;
using Identity.Application.UseCases.Query.Users;
using Identity.Presentation.Shared.Models.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{username}/info")]
    [Authorize]
    public async Task<IActionResult> GetUserByName([FromRoute] string username, CancellationToken cancellationToken)
    {
        UserDTO result = await _mediator.Send(new GetUserByNameQuery() { Username = username }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("current/info")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserInfo(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCurrentUserInfoQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost("{authorId:guid}/follow")]
    [Authorize]
    public async Task<IActionResult> Follow([FromRoute] Guid authorId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new FollowCommand() { AuthorId = authorId }, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{authorId:guid}/follow")]
    [Authorize]
    public async Task<IActionResult> Unfollow([FromRoute] Guid authorId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UnfollowCommand() { AuthorId = authorId }, cancellationToken);

        return NoContent();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(_mapper.Map<UpdateUserCommand>(request), cancellationToken);

        return NoContent();
    }

    [HttpPut("avatar")]
    [Authorize]
    public async Task<IActionResult> UpdateAvatarAsync(IFormFile formFile, CancellationToken cancellationToken)
    {
        var command = new UpdateAvatarCommand()
        {
            ImageStream = formFile.OpenReadStream(),
            FileName = formFile.FileName,
        };

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUsersByPartialName([FromRoute] string username, [FromQuery] int pageNo, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetShortUsersInfoByPartialNameQuery()
        {
            PartialUsername = username,
            PageNo = pageNo,
            PageSize = pageSize
        }, cancellationToken);

        return Ok(result);
    }
}
