using AutoMapper;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Command.Answer;
using Forum.Application.UseCase.Query.Answer;
using Forum.Infrastructure.SignalR.Hubs;
using Forum.Presentation.Shared.Models.DTOs.Answer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Forum.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnswersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHubContext<QuestionsHub> _questionsHub;
    private readonly IMapper _mapper;

    public AnswersController(IMediator mediator, IMapper mapper, IHubContext<QuestionsHub> hubContext)
    {
        _mediator = mediator;
        _mapper = mapper;
        _questionsHub = hubContext;
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
        var response = await _mediator.Send(
            _mapper.Map<AddAnswerCommand>(addAnswerRequest), 
            cancellationToken);

        await _questionsHub.Clients
            .Group(response.QuestionId.ToString())
            .SendAsync("AnswerCreatedHandler", response, cancellationToken);

        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAnswer([FromBody] UpdateAnswerRequestDTO updateAnswerRequest,  CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            _mapper.Map<UpdateAnswerCommand>(updateAnswerRequest), 
            cancellationToken);

        await _questionsHub.Clients
            .Group(response.QuestionId.ToString())
            .SendAsync("AnswerUpdatedHandler", response, cancellationToken);

        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAnswer([FromBody] DeleteAnswerRequestDTO deleteAnswerRequest,  CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            _mapper.Map<DeleteAnswerCommand>(deleteAnswerRequest), 
            cancellationToken);

        await _questionsHub.Clients
            .Group(response.QuestionId.ToString())
            .SendAsync("AnswerDeletedHandler", response.AnswerId, cancellationToken);

        return NoContent();
    }
}
