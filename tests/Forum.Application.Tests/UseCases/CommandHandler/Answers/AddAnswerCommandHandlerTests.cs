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

namespace Forum.Application.UnitTests.UseCase.CommandHandlers.Answers
{
    public class AddAnswerCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ITransactionProvider> _transactionProviderMock;
        private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
        private readonly AddAnswerCommandHandler _handler;

        public AddAnswerCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _transactionProviderMock = new Mock<ITransactionProvider>();
            _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
            _handler = new AddAnswerCommandHandler(
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

            var command = new AddAnswerCommand { QuestionId = Guid.NewGuid(), Text = "Test" };

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

            var command = new AddAnswerCommand { QuestionId = questionId, Text = "Test" };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardNotFoundException>()
                .WithMessage($"To create an answer, question with id {questionId} was not found");
        }

        [Fact]
        public async Task Handle_WhenQuestionIsClosed_ShouldThrowGuardArgumentException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Question { Id = questionId, IsClosed = true });

            var command = new AddAnswerCommand { QuestionId = questionId, Text = "Test" };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardArgumentException>()
                .WithMessage($"Question with id {questionId} closed");
        }

        [Fact]
        public async Task Handle_WhenUserNotFound_ShouldThrowGuardNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Question { Id = questionId, IsClosed = false });

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var command = new AddAnswerCommand { QuestionId = questionId, Text = "Test" };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardNotFoundException>()
                .WithMessage($"User with id {userId} was not found");
        }

        [Fact]
        public async Task Handle_WhenValidRootAnswer_ShouldCreateAnswerAndReturnDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var user = new User { Id = userId, Username = "test", PublicUsername = "public" };
            var answer = new Answer { Id = Guid.NewGuid(), AuthorId = userId, QuestionId = questionId };
            var answerDto = new AnswerDTO { Id = answer.Id, AuthorId = userId, QuestionId = questionId };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId, Username = user.Username, PublicUsername = user.PublicUsername });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Question { Id = questionId, IsClosed = false });

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _mapperMock.Setup(x => x.Map<Answer>(It.IsAny<AddAnswerCommand>()))
                .Returns(answer);

            _mapperMock.Setup(x => x.Map<AnswerDTO>(answer))
                .Returns(answerDto);

            _unitOfWorkMock.Setup(x => x.AnswerRepository.CreateAnswerAsync(answer, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(answer.Id));

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new AddAnswerCommand { QuestionId = questionId, Text = "Test" };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(answerDto);
            result.AuthorUsername.Should().Be(user.Username);
            result.PublicAuthorUsername.Should().Be(user.PublicUsername);
            result.IsAuthor.Should().BeTrue();

            _transactionProviderMock.Verify(x => x.OpenTransaction(It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.AnswerRepository.CreateAnswerAsync(answer, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _transactionProviderMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenValidChildAnswer_ShouldSetCorrectLevel()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var user = new User { Id = userId, Username = "test", PublicUsername = "public" };
            var parentAnswer = new Answer { Id = parentId, Level = 2 };
            var answer = new Answer { Id = Guid.NewGuid(), AuthorId = userId, QuestionId = questionId, ParentId = parentId };
            var answerDto = new AnswerDTO { Id = answer.Id, AuthorId = userId, QuestionId = questionId, ParentId = parentId };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = userId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdAsync(questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Question { Id = questionId, IsClosed = false });

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdAsync(parentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(parentAnswer);

            _mapperMock.Setup(x => x.Map<Answer>(It.IsAny<AddAnswerCommand>()))
                .Returns(answer);

            _mapperMock.Setup(x => x.Map<AnswerDTO>(answer))
                .Returns(answerDto);

            var command = new AddAnswerCommand { QuestionId = questionId, Text = "Test", ParentId = parentId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            answer.Level.Should().Be(parentAnswer.Level + 1);
        }
    }
}