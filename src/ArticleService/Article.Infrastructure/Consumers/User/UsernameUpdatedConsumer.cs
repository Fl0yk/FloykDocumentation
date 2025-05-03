namespace Article.Infrastructure.Consumers.User;

//TODO: нужен ли ивент? Переписать полностью
//public class UsernameUpdatedConsumer : IConsumer<UsernameUpdated>
//{
//    private readonly IMediator _mediator;
//    private readonly IMapper _mapper;

//    public UsernameUpdatedConsumer(IMediator mediator, IMapper mapper)
//    {
//        _mediator = mediator;
//        _mapper = mapper;
//    }

//    public async Task Consume(ConsumeContext<UsernameUpdated> context)
//    {
//        var request = _mapper.Map<UpdateUsernameForAuthorsRequest>(context.Message);

//        await _mediator.Send(request, context.CancellationToken);
//    }
//}
