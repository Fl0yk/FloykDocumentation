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

    [HttpGet("{username}")]
    [Authorize]
    public async Task<IActionResult> GetUserByName([FromRoute] string username, CancellationToken cancellationToken)
    {
        UserDTO result = await _mediator.Send(new GetUserByNameQuery() { Username = username }, cancellationToken);

        return Ok(result);
    }

    [HttpPost("follow/{authorId:guid}")]
    [Authorize]
    public async Task<IActionResult> Follow([FromRoute]Guid authorId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new FollowCommand() { AuthorId = authorId }, cancellationToken);

        return NoContent();
    }

    [HttpDelete("follow/{authorId:guid}")]
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

    //[HttpPost("saved-article/{articleId:guid}")]
    //[Authorize]
    //public async Task<IActionResult> SaveArticlePost([FromRoute] Guid articleId, CancellationToken cancellationToken)
    //{
    //    await _userService.SaveArticleAsync(new SaveArticleRequest(articleId), cancellationToken);

    //    return NoContent();
    //}

    //[HttpDelete("saved-article/{articleId:guid}")]
    //[Authorize]
    //public async Task<IActionResult> DeleteSavedArticle([FromRoute] Guid articleId, CancellationToken cancellationToke)
    //{
    //    await _userService.RemoveSavedArticleAsync(articleId, cancellationToke);

    //    return NoContent();
    //}
}
