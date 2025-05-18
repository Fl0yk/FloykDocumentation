using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Query.Question;
using Forum.Application.UseCase.QueryHandlers.Question;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using Moq;

namespace Forum.Application.UnitTests.UseCase.QueryHandlers.Questions
{
    public class GetQuestionByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
        private readonly GetQuestionByIdQueryHandler _handler;

        public GetQuestionByIdQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
            _handler = new GetQuestionByIdQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _currentUserProviderMock.Object);
        }

        [Fact]
        public async Task Handle_WhenQuestionNotFound_ShouldThrowGuardNotFoundException()
        {
            // Arrange
            var questionId = Guid.NewGuid();

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdWithAnswersAsync(
                questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Question)null);

            var query = new GetQuestionByIdQuery { Id = questionId };

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardNotFoundException>()
                .WithMessage($"Question with id {questionId} not found");
        }

        [Fact]
        public async Task Handle_WhenAuthorNotFound_ShouldThrowGuardNotFoundException()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var question = new Question { Id = questionId, AuthorId = Guid.NewGuid() };

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdWithAnswersAsync(
                questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(
                question.AuthorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var query = new GetQuestionByIdQuery { Id = questionId };

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardNotFoundException>()
                .WithMessage("Author");
        }

        [Fact]
        public async Task Handle_WhenValidRequest_ShouldReturnQuestionDtoWithAuthorInfo()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var authorId = Guid.NewGuid();
            var question = new Question
            {
                Id = questionId,
                AuthorId = authorId,
                Answers = new List<Answer>()
            };
            var author = new User
            {
                Id = authorId,
                Username = "testuser",
                PublicUsername = "publicuser"
            };
            var expectedDto = new QuestionDTO { Id = questionId };

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdWithAnswersAsync(
                questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(
                authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            _mapperMock.Setup(x => x.Map<QuestionDTO>(question))
                .Returns(expectedDto);

            var query = new GetQuestionByIdQuery { Id = questionId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedDto);
            result.AuthorUsername.Should().Be("testuser");
            result.PublicAuthorUsername.Should().Be("publicuser");
            result.IsAuthor.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_WhenCurrentUserIsAuthor_ShouldSetIsAuthorToTrue()
        {
            // Arrange
            var currentUserId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var question = new Question
            {
                Id = questionId,
                AuthorId = currentUserId,
                Answers = new List<Answer>()
            };
            var author = new User
            {
                Id = currentUserId,
                Username = "testuser",
                PublicUsername = "publicuser"
            };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = currentUserId });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdWithAnswersAsync(
                questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(
                currentUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            _mapperMock.Setup(x => x.Map<QuestionDTO>(question))
                .Returns(new QuestionDTO { Id = questionId });

            var query = new GetQuestionByIdQuery { Id = questionId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsAuthor.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldSortAnswersUsingAnswerComparator()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var answer1 = new Answer { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.Now.AddDays(-1) };
            var answer2 = new Answer { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.Now };
            var question = new Question
            {
                Id = questionId,
                AuthorId = Guid.NewGuid(),
                Answers = new List<Answer> { answer1, answer2 }
            };
            var author = new User { Id = question.AuthorId };

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdWithAnswersAsync(
                questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(
                It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            _mapperMock.Setup(x => x.Map<QuestionDTO>(It.IsAny<Question>()))
                .Returns<Question>(q => new QuestionDTO
                {
                    Answers = q.Answers.OrderByDescending(c => c.CreatedAt).Select(a => new InnerAnswerDTO { Id = a.Id, TimeOfCreation = a.CreatedAt }).ToList()
                });

            var query = new GetQuestionByIdQuery { Id = questionId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Answers.Should().BeInAscendingOrder(a => a.Id);
            result.Answers.Should().BeInDescendingOrder(a => a.TimeOfCreation);
        }

        [Fact]
        public async Task Handle_ShouldMapAnswerAuthorsCorrectly()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var answerAuthorId = Guid.NewGuid();
            var answer = new Answer { Id = Guid.NewGuid(), AuthorId = answerAuthorId };
            var question = new Question
            {
                Id = questionId,
                AuthorId = Guid.NewGuid(),
                Answers = new List<Answer> { answer }
            };
            var questionAuthor = new User { Id = question.AuthorId };
            var answerAuthor = new User
            {
                Id = answerAuthorId,
                Username = "answeruser",
                PublicUsername = "publicanswer"
            };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser { Id = Guid.NewGuid() });

            _unitOfWorkMock.Setup(x => x.QuestionRepository.FirstOrDefaultByIdWithAnswersAsync(
                questionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(question);

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(
                question.AuthorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(questionAuthor);

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(
                answerAuthorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(answerAuthor);

            _mapperMock.Setup(x => x.Map<QuestionDTO>(It.IsAny<Question>()))
                .Returns<Question>(q => new QuestionDTO
                {
                    Answers = q.Answers.Select(a => new InnerAnswerDTO
                    {
                        Id = a.Id,
                        AuthorId = a.AuthorId
                    }).ToList()
                });

            var query = new GetQuestionByIdQuery { Id = questionId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var firstAnswer = result.Answers.First();
            firstAnswer.AuthorUsername.Should().Be("answeruser");
            firstAnswer.PublicAuthorUsername.Should().Be("publicanswer");
            firstAnswer.IsAuthor.Should().BeFalse();
        }
    }
}