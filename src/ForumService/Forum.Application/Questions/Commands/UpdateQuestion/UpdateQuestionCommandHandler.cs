﻿using AutoMapper;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using MediatR;

namespace Forum.Application.Questions.Commands.UpdateQuestion;
public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Guid>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateQuestionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _questionRepository = unitOfWork.QuestionRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        Question? dbQuestion = await _questionRepository.FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

        if (dbQuestion is null)
            throw new KeyNotFoundException($"Question with id {request.Id} not found");

        _mapper.Map(request, dbQuestion);

        Guid id = await _questionRepository.UpdateQuestionAsync(dbQuestion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return id;
    }
}