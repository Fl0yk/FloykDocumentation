using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Forum.Application.UseCase.Command.Question;
using Forum.Application.UseCase.CommandHandlers.Question;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using Moq;

namespace Forum.Application.UnitTests.UseCase.CommandHandlers.Questions
{
    public class DeleteQuestionCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITransactionProvider> _transactionProviderMock;
        private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
        private readonly DeleteQuestionCommandHandler _handler;

        public DeleteQuestionCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _transactionProviderMock = new Mock<ITransactionProvider>();
            _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
            _handler = new DeleteQuestionCommandHandler(
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

            var command = new DeleteQuestionCommand { Id = Guid.NewGuid() };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardForbiddenException>()
                .WithMessage("Current user is null");

            _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
            _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenQuestionNotFound_ShouldThrowGuardNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Question)null);

            var command = new DeleteQuestionCommand { Id = questionId };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardNotFoundException>()
                .WithMessage($"Question with id {questionId} not found");
        }

        [Fact]
        public async Task Handle_WhenUserIsNotAuthor_ShouldThrowGuardForbiddenException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var authorId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var question = new Question { Id = questionId, AuthorId = authorId };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            var command = new DeleteQuestionCommand { Id = questionId };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardForbiddenException>()
                .WithMessage($"Current user with id {userId} is not author of question with id {questionId}");
        }

        [Fact]
        public async Task Handle_WhenValidRequest_ShouldDeleteQuestion()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var question = new Question { Id = questionId, AuthorId = userId };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            _unitOfWorkMock.Setup(x => x.QuestionRepository.DeleteQuestionAsync(question, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new DeleteQuestionCommand { Id = questionId };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.QuestionRepository.DeleteQuestionAsync(
                It.Is<Question>(q => q.Id == questionId && q.AuthorId == userId),
                It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenExceptionOccurs_ShouldNotCommitTransaction()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var question = new Question { Id = questionId, AuthorId = userId };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            _unitOfWorkMock.Setup(x => x.QuestionRepository.DeleteQuestionAsync(question, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            var command = new DeleteQuestionCommand { Id = questionId };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
            _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}