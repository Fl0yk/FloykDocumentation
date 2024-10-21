using AutoMapper;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Command.Answer;
using Forum.Application.UseCase.Query.Answer;
using Forum.Presentation.Shared.Models.DTOs.Answer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnswersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AnswersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAnswerById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        AnswerDTO answer = await _mediator.Send(new GetAnswerByIdQuery(id), cancellationToken);

        return Ok(answer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAnswer([FromBody] AddAnswerRequestDTO addAnswerRequest,  CancellationToken cancellationToken = default)
    {
        Guid answerId = await _mediator.Send(
            _mapper.Map<AddAnswerCommand>(addAnswerRequest), 
            cancellationToken);

        return Ok(answerId);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAnswer([FromBody] UpdateAnswerRequestDTO updateAnswerRequest,  CancellationToken cancellationToken = default)
    {
        Guid answerId = await _mediator.Send(
            _mapper.Map<UpdateAnswerCommand>(updateAnswerRequest), 
            cancellationToken);

        return Ok(answerId);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAnswer([FromRoute] Guid id,  CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteAnswerCommand(id), cancellationToken);

        return NoContent();
    }
}
