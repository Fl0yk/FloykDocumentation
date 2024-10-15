using Forum.Application.Answers.Commands.CreateAnswer;
using Forum.Application.Answers.Commands.DeleteAnswer;
using Forum.Application.Answers.Commands.UpdateAnswer;
using Forum.Application.Answers.Queries;
using Forum.Application.Shared.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnswersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnswersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAnswerById(
                            [FromRoute] Guid id, 
                            CancellationToken cancellationToken = default)
    {
        AnswerDTO answer = await _mediator.Send(new GetAnswerByIdQuery(id), cancellationToken);

        return Ok(answer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAnswer(
                            [FromBody] CreateAnswerCommand createAnswerCommand, 
                            CancellationToken cancellationToken = default)
    {
        Guid answerId = await _mediator.Send(createAnswerCommand, cancellationToken);

        return Ok(answerId);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAnswer(
                            [FromBody] UpdateAnswerCommand updateAnswerCommand, 
                            CancellationToken cancellationToken = default)
    {
        Guid answerId = await _mediator.Send(updateAnswerCommand, cancellationToken);

        return Ok(answerId);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAnswer(
                            [FromRoute] Guid id, 
                            CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteAnswerCommand(id), cancellationToken);

        return NoContent();
    }
}
