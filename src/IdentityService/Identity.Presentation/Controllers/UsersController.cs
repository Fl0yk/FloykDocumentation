﻿using AutoMapper;
using Identity.Application.Abstractions.Services;
using Identity.Application.Services.Requests.UserRequests;
using Identity.Application.Shared.Models.DTOs;
using Identity.Presentation.Shared.Models.DTOs.User;
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

    [HttpGet("{username}")]
    [Authorize]
    public async Task<IActionResult> GetUserByName([FromRoute] string username, CancellationToken cancellationToken)
    {
        UserDTO result = await _userService.GetUserByNameAsync(username, cancellationToken);

        return Ok(result);
    }

    [HttpPost("follow/{authorId:guid}")]
    [Authorize]
    public async Task<IActionResult> FollowPost([FromRoute]Guid authorId, CancellationToken cancellationToken)
    {
        await _userService.FollowAsync(authorId, cancellationToken);

        return NoContent();
    }

    [HttpDelete("unfollow/{authorId:guid}")]
    [Authorize]
    public async Task<IActionResult> UnfollowPost([FromRoute] Guid authorId, CancellationToken cancellationToken)
    {
        await _userService.UnfollowAsync(authorId, cancellationToken);

        return NoContent();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateUserPost([FromBody] UpdateUserRequestDTO request, CancellationToken cancellationToken)
    {
        await _userService.UpdateUserAsync(_mapper.Map<UpdateUserRequest>(request), cancellationToken);

        return NoContent();
    }

    [HttpPut("avatar")]
    [Authorize]
    public async Task<IActionResult> UpdateAvatarAsync(IFormFile formFile, CancellationToken cancellationToken)
    {
        await _userService.UpdateAvatarAsync(new(formFile.FileName, formFile.OpenReadStream()), cancellationToken);

        return NoContent();
    }

    [HttpPost("saved-article")]
    [Authorize]
    public async Task<IActionResult> SaveArticlePost([FromBody] SaveArticleRequestDTO request, CancellationToken cancellationToken)
    {
        await _userService.SaveArticleAsync(_mapper.Map<SaveArticleRequest>(request), cancellationToken);

        return NoContent();
    }

    [HttpDelete("saved-article/{articleId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteSavedArticle([FromRoute] Guid articleId, CancellationToken cancellationToke)
    {
        await _userService.RemoveSavedArticleAsync(articleId, cancellationToke);

        return NoContent();
    }
}
