using Article.Application.UseCases.Comand.Articles;
using Article.Application.UseCases.Query.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;
using Core.Constants;
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

    public ArticlesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("paginated/date")]
    public async Task<IActionResult> GetPaginatedByDate([FromQuery] GetPaginatedByDateArticlesRequestDTO request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            _mapper.Map< GetPaginatedByDateShortArticlesQuery>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("paginated/author")]
    public async Task<IActionResult> GetPaginatedByName([FromQuery] GetPaginatedByAuthorArticlesRequestDTO request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            _mapper.Map<GetPaginatedByAuthorNameShortArticlesQuery>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("paginated/category")]
    public async Task<IActionResult> GetPaginatedByCategory([FromQuery] GetPaginatedByCategoryArticlesRequestDTO request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            _mapper.Map<GetPaginatedByCategoryShortArticlesQuery>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetArticleByIdQuery() { Id = id }, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateArticle([FromBody] CreateArticleRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<CreateArticleCommand>(request), 
            cancellationToken);

        return NoContent();
    }

    [HttpPost("block")]
    [Authorize]
    public async Task<IActionResult> AppendBlock([FromBody] AppendBlockRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<AppendBlockCommand>(request), 
            cancellationToken);

        return NoContent();
    }

    [HttpPost("publish")]
    [Authorize(Roles = Roles.Author)]
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
}
