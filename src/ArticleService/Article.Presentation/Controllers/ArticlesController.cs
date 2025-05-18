using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Comand.Articles;
using Article.Application.UseCases.Query.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;
using Core.Constants;
using Core.Managers;
using Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Article.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IImageManager _imageManager;

    public ArticlesController(IMediator mediator, IMapper mapper, IImageManager imageManager)
    {
        _mediator = mediator;
        _mapper = mapper;
        _imageManager = imageManager;
    }

    [HttpGet("paginated/date")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<ShortArticleDTO>))]
    public async Task<IActionResult> GetPaginatedByDate([FromQuery] GetPaginatedByDateArticlesRequestDTO request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            _mapper.Map<GetPaginatedByDateShortArticlesQuery>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("paginated/author")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<ShortArticleDTO>))]
    public async Task<IActionResult> GetPaginatedByName([FromQuery] GetPaginatedByAuthorArticlesRequestDTO request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Paginated by author in repository work for cuurent users. Method return not published articles");
        var result = await _mediator.Send(
            _mapper.Map<GetPaginatedByAuthorNameShortArticlesQuery>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("paginated/current")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<ShortArticleDTO>))]
    public async Task<IActionResult> GetCurrentUserArticles([FromQuery] GetCurrentUserShortArticlesRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            _mapper.Map<GetCurrentUserShortArticlesQuery>(request),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("paginated/popular")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<ShortArticleDTO>))]
    public async Task<IActionResult> GetPopularPaginated([FromQuery] GetPopularPaginatedArticlesRequestDTO request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            _mapper.Map<GetPopularPaginatedShortArticlesQuery>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleDTO))]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetArticleByIdQuery() { Id = id }, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateArticle([FromBody] CreateArticleRequestDTO request, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            _mapper.Map<CreateArticleCommand>(request), 
            cancellationToken);

        return Ok(id);
    }

    [HttpPost("block")]
    [Authorize]
    public async Task<IActionResult> AppendBlock([FromBody] AppendBlockCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPost("publish")]
    [Authorize]
    public async Task<IActionResult> PublishArticle([FromBody] PublishArticleRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<PublishArticleCommand>(request), 
            cancellationToken);

        return NoContent();
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateArticle([FromBody] UpdateArticleRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<UpdateArticleCommand>(request), 
            cancellationToken);

        return NoContent();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteArticle([FromBody] DeleteArticleRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<DeleteArticleCommand>(request), 
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("block")]
    [Authorize]
    public async Task<IActionResult> DeleteBlock([FromBody] DeleteBlockRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<DeleteBlockCommand>(request), 
            cancellationToken);

        return NoContent();
    }

    [HttpGet("approve")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetShouldBeApprovedArticles(CancellationToken cancellationToken)
    {
        var res = await _mediator.Send(
            new GetShouldBeApprovedArticlesQuery(),
            cancellationToken);

        return Ok(res);
    }

    [HttpPost("{articleId:guid}/approve")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ApproveArticle([FromRoute] Guid articleId, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ApproveArticleCommand()
            {
                ArticleId = articleId
            },
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{articleId:guid}/save")]
    [Authorize]
    public async Task<IActionResult> SaveArticle([FromRoute] Guid articleId, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new SaveArticleCommand()
            {
                ArticleId = articleId
            },
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{articleId:guid}/unsave")]
    [Authorize]
    public async Task<IActionResult> UnsaveArticle([FromRoute] Guid articleId, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UnsaveArticleCommand()
            {
                ArticleId = articleId
            },
            cancellationToken);

        return NoContent();
    }

    [HttpGet("saved-articles")]
    [Authorize]
    public async Task<IActionResult> GetSavedArticles(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSavedArticlesByUserQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost("image")]
    public async Task<IActionResult> UploadImage([FromQuery] string? oldFileUrl, [FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        var url = await _imageManager.SaveImageAsync(file.OpenReadStream(), file.FileName, oldFileUrl, cancellationToken);

        return Ok(url);
    }
}
