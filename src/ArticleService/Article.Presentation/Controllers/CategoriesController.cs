using Article.Application.UseCases.Requests.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Article.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCategoriesRequest());

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetArticleById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoryByIdWithoutArticlesRequest(id), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostCategory(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCategoryRequest(id), cancellationToken);

        return NoContent();
    }


}
