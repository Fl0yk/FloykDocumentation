using Article.Application.UseCases.Requests.Categories;
using Article.Presentation.Shared.Models.DTOs.Category;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Article.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CategoriesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCategoriesRequest(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetArticleById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetCategoryByIdWithoutArticlesRequest(id), 
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> PostCategory(CreateCategoryRequestDTO request, CancellationToken cancellationToken)
    {
        await _mediator.Send(
            _mapper.Map<CreateCategoryRequest>(request), 
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCategoryRequest(id), cancellationToken);

        return NoContent();
    }


}
