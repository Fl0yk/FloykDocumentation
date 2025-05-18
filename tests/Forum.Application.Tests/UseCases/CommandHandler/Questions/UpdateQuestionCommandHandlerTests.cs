using AutoMapper;
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
    public class UpdateQuestionCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ITransactionProvider> _transactionProviderMock;
        private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
        private readonly UpdateQuestionCommandHandler _handler;

        public UpdateQuestionCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _transactionProviderMock = new Mock<ITransactionProvider>();
            _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
            _handler = new UpdateQuestionCommandHandler(
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

            var command = new UpdateQuestionCommand
            {
                Id = Guid.NewGuid(),
                Title = "Updated Title",
                Description = "Updated Description"
            };

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

            var command = new UpdateQuestionCommand
            {
                Id = questionId,
                Title = "Updated Title",
                Description = "Updated Description"
            };

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

            var command = new UpdateQuestionCommand
            {
                Id = questionId,
                Title = "Updated Title",
                Description = "Updated Description"
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardForbiddenException>()
                .WithMessage($"Current user with id {userId} is not author of question with id {questionId}");
        }

        [Fact]
        public async Task Handle_WhenQuestionIsClosed_ShouldThrowGuardArgumentException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var question = new Question { Id = questionId, AuthorId = userId, IsClosed = true };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            var command = new UpdateQuestionCommand
            {
                Id = questionId,
                Title = "Updated Title",
                Description = "Updated Description"
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardArgumentException>()
                .WithMessage($"Question with id {questionId} closed");
        }

        [Fact]
        public async Task Handle_WhenValidRequest_ShouldUpdateQuestionAndReturnId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var question = new Question { Id = questionId, AuthorId = userId, IsClosed = false };
            var updatedQuestion = new Question { Id = questionId, AuthorId = userId, IsClosed = false };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            _mapperMock.Setup(x => x.Map(It.IsAny<UpdateQuestionCommand>(), It.IsAny<Question>()))
                .Callback<UpdateQuestionCommand, Question>((cmd, q) =>
                {
                    q.Title = cmd.Title;
                    q.Description = cmd.Description;
                });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.UpdateQuestionAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questionId);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new UpdateQuestionCommand
            {
                Id = questionId,
                Title = "Updated Title",
                Description = "Updated Description"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(questionId);
            question.Title.Should().Be("Updated Title");
            question.Description.Should().Be("Updated Description");

            _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map(command, question), Times.Once);
            _unitOfWorkMock.Verify(x => x.QuestionRepository.UpdateQuestionAsync(
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
            var question = new Question { Id = questionId, AuthorId = userId, IsClosed = false };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            _unitOfWorkMock.Setup(x => x.QuestionRepository.UpdateQuestionAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            var command = new UpdateQuestionCommand
            {
                Id = questionId,
                Title = "Updated Title",
                Description = "Updated Description"
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
            _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}