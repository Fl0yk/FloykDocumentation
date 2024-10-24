using AutoMapper;
using Identity.Application.Abstractions.Services;
using Identity.Application.Shared.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        UserDTO result = await _userService.GetUserByIdAsync(cancellationToken);

        return Ok(result);
    }

    [HttpPost("follow/{authorId:guid}")]
    [Authorize]
    public async Task<IActionResult> FollowPost([FromRoute]Guid authorId, CancellationToken cancellationToken)
    {
        await _userService.FollowAsync(authorId, cancellationToken);

        return NoContent();
    }

    [HttpPost("unfollow/{authorId:guid}")]
    [Authorize]
    public async Task<IActionResult> UnfollowPost([FromRoute] Guid authorId, CancellationToken cancellationToken)
    {
        await _userService.UnfollowAsync(authorId, cancellationToken);

        return NoContent();
    }
}
