using AutoMapper;
using Forum.Application.Shared.Models;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Command.Question;
using Forum.Application.UseCase.Query.Question;
using Forum.Presentation.Shared.Models.DTOs.Question;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public QuestionsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginatedByDateQuestions([FromQuery] GetPaginatedByDateQuestionsRequestDTO paginatedRequest , CancellationToken cancellationToken = default)
    {
        PaginatedResult<QuestionDTO> response = await _mediator.Send(
                    _mapper.Map<GetPaginatedByDateQuestionsQuery>(paginatedRequest), 
                    cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetQuestionById([FromRoute] Guid id)
    {
        QuestionDTO question = await _mediator.Send(new GetQuestionByIdQuery(id));

        return Ok(question);
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequestDTO createQuestionRequest, CancellationToken cancellationToken = default)
    {
        Guid questionId = await _mediator.Send(
            _mapper.Map<CreateQuestionCommand>(createQuestionRequest), 
            cancellationToken);

        return Ok(questionId);
    }

    [HttpPost("close/{id:guid}")]
    public async Task<IActionResult> CloseQuestion([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        Guid questionId = await _mediator.Send(new CloseQuestionCommand(id), cancellationToken);

        return Ok(questionId);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionRequestDTO updateQuestionRequest, CancellationToken cancellationToken = default)
    {
        Guid questionId = await _mediator.Send(
            _mapper.Map<UpdateQuestionCommand>(updateQuestionRequest), 
            cancellationToken);

        return Ok(questionId);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteQuestion([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteQuestionCommand(id));

        return NoContent();
    }
}