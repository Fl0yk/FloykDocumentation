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
    public class CloseQuestionCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITransactionProvider> _transactionProviderMock;
        private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
        private readonly CloseQuestionCommandHandler _handler;

        public CloseQuestionCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _transactionProviderMock = new Mock<ITransactionProvider>();
            _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
            _handler = new CloseQuestionCommandHandler(
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

            var command = new CloseQuestionCommand { Id = Guid.NewGuid() };

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

            var command = new CloseQuestionCommand { Id = questionId };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardNotFoundException>()
                .WithMessage($"Question with id {questionId} not found");
        }

        [Fact]
        public async Task Handle_WhenQuestionAlreadyClosed_ShouldThrowGuardArgumentException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var question = new Question { Id = questionId, IsClosed = true, AuthorId = userId };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            var command = new CloseQuestionCommand { Id = questionId };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardArgumentException>()
                .WithMessage($"Question with id {questionId} is already closed");
        }

        [Fact]
        public async Task Handle_WhenUserIsNotAuthor_ShouldThrowGuardForbiddenException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var authorId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var question = new Question { Id = questionId, IsClosed = false, AuthorId = authorId };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            var command = new CloseQuestionCommand { Id = questionId };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardForbiddenException>()
                .WithMessage($"Current user with id {userId} is not author of question with id {questionId}");
        }

        [Fact]
        public async Task Handle_WhenValidRequest_ShouldCloseQuestionAndReturnId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var question = new Question { Id = questionId, IsClosed = false, AuthorId = userId };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            _unitOfWorkMock.Setup(x => x.QuestionRepository.UpdateQuestionAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questionId);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new CloseQuestionCommand { Id = questionId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(questionId);
            question.IsClosed.Should().BeTrue();

            _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.QuestionRepository.UpdateQuestionAsync(
                It.Is<Question>(q => q.IsClosed && q.Id == questionId),
                It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}