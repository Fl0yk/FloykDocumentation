using Forum.Application.Questions.Commands.CloseQuestion;
using Forum.Application.Questions.Commands.CreateQuestion;
using Forum.Application.Questions.Commands.DeleteQuestion;
using Forum.Application.Questions.Commands.UpdateQuestion;
using Forum.Application.Questions.Queries.GetPaginatedByDateQuestions;
using Forum.Application.Questions.Queries.GetQuestionById;
using Forum.Application.Shared.Models;
using Forum.Application.Shared.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuestionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginatedByDateQuestions(
                            [FromQuery] int pageNumber = 1, 
                            [FromQuery] int pageSize = 3, 
                            CancellationToken cancellationToken = default)
    {
        PaginatedResult<QuestionDTO> response = await _mediator.Send(
                    new GetPaginatedByDateQuestionsQuery(pageSize, pageNumber), 
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
    public async Task<IActionResult> CreateQuestion(
                            [FromBody] CreateQuestionCommand createQuestionCommand,
                            CancellationToken cancellationToken = default)
    {
        Guid questionId = await _mediator.Send(createQuestionCommand, cancellationToken);

        return Ok(questionId);
    }

    [HttpPost("close/{id:guid}")]
    public async Task<IActionResult> CloseQuestion(
                            [FromRoute] Guid id, 
                            CancellationToken cancellationToken = default)
    {
        Guid questionId = await _mediator.Send(new CloseQuestionCommand(id), cancellationToken);

        return Ok(questionId);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateQuestion(
                            [FromBody] UpdateQuestionCommand updateQuestionCommand,
                            CancellationToken cancellationToken = default)
    {
        Guid questionId = await _mediator.Send(updateQuestionCommand, cancellationToken);

        return Ok(questionId);
    }

    [HttpDelete("/{id:guid}")]
    public async Task<IActionResult> DeleteQuestion(
                            [FromRoute] Guid id,
                            CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteQuestionCommand(id));

        return NoContent();
    }
}