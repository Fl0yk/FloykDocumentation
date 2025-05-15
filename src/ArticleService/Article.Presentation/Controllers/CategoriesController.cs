using Article.Application.Shared.Models.DTOs;
using Article.Application.UseCases.Query.Categories;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryDTO>))]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetArticleById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCategoryByIdWithoutArticlesQuery() { Id = id }, 
            cancellationToken);

        return Ok(result);
    }
}
