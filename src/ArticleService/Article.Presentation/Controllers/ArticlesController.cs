using Article.Application.UseCases.Requests.Articles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Article.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ArticlesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArticlesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("paginated/date")]
    public async Task<IActionResult> GetPaginatedByDate([FromQuery]GetPaginatedByDateShortArticlesRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet("paginated/author")]
    public async Task<IActionResult> GetPaginatedByName([FromQuery] GetPaginatedByAuthorNameShortArticlesRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet("paginated/category")]
    public async Task<IActionResult> GetPaginatedByCategory([FromQuery] GetPaginatedByCategoryShortArticlesRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetArticleByIdRequest(id), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostArticle(CreateArticleRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPost("block")]
    public async Task<IActionResult> PostBlock(AppendBlockRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPost("publish")]
    public async Task<IActionResult> PublishArticle(PublishArticleRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateArticle(UpdateArticleRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteArticle([FromBody] DeleteArticleRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }

    [HttpDelete("block")]
    public async Task<IActionResult> DeleteBlock([FromBody]DeleteBlockRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);

        return NoContent();
    }
}
