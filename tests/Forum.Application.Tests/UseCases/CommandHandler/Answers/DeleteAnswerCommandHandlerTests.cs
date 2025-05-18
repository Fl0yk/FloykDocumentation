using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Forum.Application.Shared.Models.Responses;
using Forum.Application.UseCase.Command.Answer;
using Forum.Application.UseCase.CommandHandlers.Answer;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using Moq;

namespace Forum.Application.UnitTests.UseCase.CommandHandlers.Answers;

public class DeleteAnswerCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITransactionProvider> _transactionProviderMock;
    private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
    private readonly DeleteAnswerCommandHandler _handler;

    public DeleteAnswerCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _transactionProviderMock = new Mock<ITransactionProvider>();
        _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
        _handler = new DeleteAnswerCommandHandler(
            _unitOfWorkMock.Object,
            _transactionProviderMock.Object,
            _currentUserProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCurrentUserIsNull_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        _currentUserProviderMock.Setup(x => x.GetCurrentUser())
            .Returns((CurrentUser)null);

        var command = new DeleteAnswerCommand { Id = Guid.NewGuid() };

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
        var currentUser = new CurrentUser { Id = userId };
        var answerId = Guid.NewGuid();

        _currentUserProviderMock.Setup(x => x.GetCurrentUser())
            .Returns(currentUser);

        _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdWithChildrenAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Answer)null);

        var command = new DeleteAnswerCommand { Id = answerId };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardNotFoundException>()
            .WithMessage($"Answer with id {answerId} not found");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotAuthor_ShouldThrowGuardForbiddenException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var currentUser = new CurrentUser { Id = userId };
        var answerId = Guid.NewGuid();
        var answer = new Domain.Entities.Answer
        {
            Id = answerId,
            AuthorId = authorId,
            Question = new Question { Id = Guid.NewGuid(), IsClosed = false }
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser())
            .Returns(currentUser);

        _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdWithChildrenAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var command = new DeleteAnswerCommand { Id = answerId };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardForbiddenException>()
            .WithMessage($"User with id {userId} is not the author of the answer");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenQuestionIsClosed_ShouldThrowGuardArgumentException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var currentUser = new CurrentUser { Id = userId };
        var answerId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var answer = new Domain.Entities.Answer
        {
            Id = answerId,
            AuthorId = userId,
            Question = new Question { Id = questionId, IsClosed = true }
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser())
            .Returns(currentUser);

        _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdWithChildrenAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var command = new DeleteAnswerCommand { Id = answerId };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<GuardArgumentException>()
            .WithMessage($"Question with id {questionId} closed");

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenAllConditionsMet_ShouldDeleteAnswerAndReturnResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var currentUser = new CurrentUser { Id = userId };
        var answerId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var answer = new Domain.Entities.Answer
        {
            Id = answerId,
            AuthorId = userId,
            Question = new Question { Id = questionId, IsClosed = false }
        };

        _currentUserProviderMock.Setup(x => x.GetCurrentUser())
            .Returns(currentUser);

        _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdWithChildrenAsync(answerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        _unitOfWorkMock.Setup(x => x.AnswerRepository.DeleteAnswerAsync(answer, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new DeleteAnswerCommand { Id = answerId };
        var expectedResponse = new DeleteAnswerResponse { AnswerId = answerId, QuestionId = questionId };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);

        _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.AnswerRepository.DeleteAnswerAsync(answer, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
}