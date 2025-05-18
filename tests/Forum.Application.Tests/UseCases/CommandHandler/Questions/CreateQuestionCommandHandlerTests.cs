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
    public class CreateQuestionCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ITransactionProvider> _transactionProviderMock;
        private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
        private readonly CreateQuestionCommandHandler _handler;

        public CreateQuestionCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _transactionProviderMock = new Mock<ITransactionProvider>();
            _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
            _handler = new CreateQuestionCommandHandler(
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

            var command = new CreateQuestionCommand
            {
                Title = "Test Question",
                Description = "Test Description"
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
        public async Task Handle_WhenUserNotFound_ShouldThrowGuardNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var command = new CreateQuestionCommand
            {
                Title = "Test Question",
                Description = "Test Description"
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardNotFoundException>()
                .WithMessage($"User with id {userId} was not found");
        }

        [Fact]
        public async Task Handle_WhenValidRequest_ShouldCreateQuestionAndReturnId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var user = new User { Id = userId };
            var question = new Question { Id = questionId, AuthorId = userId };
            var command = new CreateQuestionCommand
            {
                Title = "Test Question",
                Description = "Test Description"
            };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _mapperMock.Setup(x => x.Map<Question>(command))
                .Returns(question);

            _unitOfWorkMock.Setup(x => x.QuestionRepository.CreateQuestionAsync(question, It.IsAny<CancellationToken>()))
                .ReturnsAsync(questionId);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(questionId);
            question.AuthorId.Should().Be(userId);

            _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<Question>(command), Times.Once);
            _unitOfWorkMock.Verify(x => x.QuestionRepository.CreateQuestionAsync(
                It.Is<Question>(q => q.AuthorId == userId),
                It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenValidRequest_ShouldSetAuthorIdFromCurrentUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var user = new User { Id = userId };
            var question = new Question { Id = questionId };
            var command = new CreateQuestionCommand
            {
                Title = "Test Question",
                Description = "Test Description"
            };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _mapperMock.Setup(x => x.Map<Question>(command))
                .Returns(question);

            _unitOfWorkMock.Setup(x => x.QuestionRepository.CreateQuestionAsync(question, It.IsAny<CancellationToken>()))
                .ReturnsAsync(questionId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            question.AuthorId.Should().Be(userId);
        }
    }
}