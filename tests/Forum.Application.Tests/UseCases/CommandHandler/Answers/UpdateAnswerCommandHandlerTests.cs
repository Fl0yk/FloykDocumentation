using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Command.Answer;
using Forum.Application.UseCase.CommandHandlers.Answer;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using Moq;

namespace Forum.Application.UnitTests.UseCase.CommandHandlers.Answers;

public class UpdateAnswerCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ITransactionProvider> _transactionProviderMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly UpdateAnswerCommandHandler _handler;

    public UpdateAnswerCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _transactionProviderMock = new Mock<ITransactionProvider>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _handler = new UpdateAnswerCommandHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _transactionProviderMock.Object,
            _currentUserProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        _currentUserProviderMock.Setup(x => x.GetCurrentUser())
            .Returns((CurrentUser)null);

        var command = new UpdateAnswerCommand { Id = Guid.NewGuid(), Text = "Updated text" };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage("Current user is null");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenAnswerNotFound_ShouldThrowGuardNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var answerId = Guid.NewGuid();
        _currentUserProviderMock.Setup(x => x.GetCurrentUser())
            .Returns(new CurrentUser { Id = userId });

        _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Answer)null);

        var command = new UpdateAnswerCommand { Id = answerId, Text = "Updated text" };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Answer with id {answerId} is not found");
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthor_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var answerId = Guid.NewGuid();
        var answer = new Answer
        {
            Id = answerId,
            AuthorId = authorId,
            Question = new Question { Id = Guid.NewGuid(), IsClosed = false }
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser())
            .Returns(new CurrentUser { Id = userId });

        _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var command = new UpdateAnswerCommand { Id = answerId, Text = "Updated text" };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage($"User with id {userId} is not the author of the answer");
    }

    [Fact]
    public async Task Handle_WhenQuestionIsClosed_ShouldThrowGuardArgumentException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var answerId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var answer = new Answer
        {
            Id = answerId,
            AuthorId = userId,
            Question = new Question { Id = questionId, IsClosed = true }
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser())
            .Returns(new CurrentUser { Id = userId });

        _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var command = new UpdateAnswerCommand { Id = answerId, Text = "Updated text" };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardArgumentException>()
            .WithMessage($"Question with id {questionId} closed");
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldUpdateAnswerAndReturnDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var answerId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var currentUser = new CurrentUser
        {
            Id = userId,
            Username = "testuser",
            PublicUsername = "publicuser"
        };

        var answer = new Answer
        {
            Id = answerId,
            AuthorId = userId,
            Text = "Original text",
            Question = new Question { Id = questionId, IsClosed = false }
        };

        var updatedAnswer = new Answer
        {
            Id = answerId,
            AuthorId = userId,
            Text = "Updated text",
            Question = new Question { Id = questionId, IsClosed = false }
        };

        var expectedDto = new AnswerDTO
        {
            Id = answerId,
            AuthorId = userId,
            Text = "Updated text",
            QuestionId = questionId,
            AuthorUsername = currentUser.Username,
            PublicAuthorUsername = currentUser.PublicUsername,
            IsAuthor = true
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser())
            .Returns(currentUser);

        _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        _unitOfWorkMock.Setup(x => x.AnswerRepository.UpdateAnswerAsync(It.IsAny<Answer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(answerId);

        _mapperMock.Setup(x => x.Map(It.IsAny<UpdateAnswerCommand>(), It.IsAny<Answer>()))
            .Callback<UpdateAnswerCommand, Answer>((cmd, ans) => ans.Text = cmd.Text);

        _mapperMock.Setup(x => x.Map<AnswerDTO>(It.IsAny<Answer>()))
            .Returns(expectedDto);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new UpdateAnswerCommand { Id = answerId, Text = "Updated text" };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedDto);

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.AnswerRepository.UpdateAnswerAsync(It.Is<Answer>(a => a.Text == "Updated text"),
            It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);

        _mapperMock.Verify(x => x.Map(command, answer), Times.Once);
        _mapperMock.Verify(x => x.Map<AnswerDTO>(It.IsAny<Answer>()), Times.Once);
    }
}