using Article.Application.UseCases.Interactors.Article;
using Article.Application.UseCases.Requests.Articles;
using Article.Presentation.Shared.Models.DTOs.Article;
using AutoMapper;
using MediatR;
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
            _mapper.Map< GetPaginatedByDateShortArticlesRequest>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("paginated/author")]
    public async Task<IActionResult> GetPaginatedByName([FromQuery] GetPaginatedByAuthorArticlesRequestDTO request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            _mapper.Map<GetPaginatedByAuthorNameShortArticlesRequest>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("paginated/category")]
    public async Task<IActionResult> GetPaginatedByCategory([FromQuery] GetPaginatedByCategoryArticlesRequestDTO request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            _mapper.Map<GetPaginatedByCategoryShortArticlesRequest>(request), 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetArticleByIdRequest(id), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostArticle(CreateArticleRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<CreateArticleRequest>(request), 
            cancellationToken);

        return NoContent();
    }

    [HttpPost("block")]
    public async Task<IActionResult> PostBlock(AppendBlockRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<AppendBlockRequest>(request), 
            cancellationToken);

        return NoContent();
    }

    [HttpPost("publish")]
    public async Task<IActionResult> PublishArticle(PublishArticleRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<PublishArticleRequest>(request), 
            cancellationToken);

        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateArticle(UpdateArticleRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteArticle([FromBody] DeleteArticleRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<DeleteArticleRequest>(request), 
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("block")]
    public async Task<IActionResult> DeleteBlock([FromBody]DeleteBlockRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<DeleteBlockRequest>(request), 
            cancellationToken);

        return NoContent();
    }
}
