using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Core.Providers.Interfaces;
using FluentAssertions;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Query.Question;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using Moq;
using System.Linq.Expressions;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Forum.Application.UseCase.QueryHandlers.Question;

namespace Forum.Application.UnitTests.UseCase.QueryHandlers.Questions
{
    public class GetPaginatedByDateQuestionQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IBaseCurrentUserProvider> _currentUserProviderMock;
        private readonly GetPaginatedByDateQuestionQueryHandler _handler;

        public GetPaginatedByDateQuestionQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _currentUserProviderMock = new Mock<IBaseCurrentUserProvider>();
            _handler = new GetPaginatedByDateQuestionQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _currentUserProviderMock.Object);
        }

        [Fact]
        public async Task Handle_WhenEmptyPage_ShouldThrowGuardArgumentException()
        {
            // Arrange
            var query = new GetPaginatedByDateQuestionsQuery { PageNumber = 2, PageSize = 10 };
            var emptyQuestions = Array.Empty<Question>();

            _unitOfWorkMock.Setup(x => x.QuestionRepository.GetQuestionsByDateAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyQuestions.AsQueryable());

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardArgumentException>()
                .WithMessage("Get an empty questions page");
        }

        [Fact]
        public async Task Handle_WhenValidRequest_ShouldReturnPaginatedResult()
        {
            // Arrange
            var currentUser = new CurrentUser { Id = Guid.NewGuid() };
            var questions = new[]
            {
                new Question { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid() },
                new Question { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid() }
            };
            var questionDtos = new[]
            {
                new QuestionDTO { Id = questions[0].Id, AuthorId = questions[0].AuthorId },
                new QuestionDTO { Id = questions[1].Id, AuthorId = questions[1].AuthorId }
            };
            var authors = new[]
            {
                new User { Id = questions[0].AuthorId, Username = "User1", PublicUsername = "Public1" },
                new User { Id = questions[1].AuthorId, Username = "User2", PublicUsername = "Public2" }
            };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(currentUser);

            _unitOfWorkMock.Setup(x => x.QuestionRepository.GetQuestionsByDateAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(questions.AsQueryable());

            _mapperMock.Setup(x => x.Map<IEnumerable<QuestionDTO>>(It.IsAny<IEnumerable<Question>>()))
                .Returns(questionDtos);

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(questions[0].AuthorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(authors[0]);

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(questions[1].AuthorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(authors[1]);

            var query = new GetPaginatedByDateQuestionsQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalPages.Should().Be(1);

            result.Items.First().AuthorUsername.Should().Be("User1");
            result.Items.First().PublicAuthorUsername.Should().Be("Public1");
            result.Items.First().IsAuthor.Should().BeFalse();

            _unitOfWorkMock.Verify(x => x.QuestionRepository.GetQuestionsByDateAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<IEnumerable<QuestionDTO>>(It.IsAny<IEnumerable<Question>>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCurrentUserIsAuthor_ShouldSetIsAuthorToTrue()
        {
            // Arrange
            var currentUserId = Guid.NewGuid();
            var currentUser = new CurrentUser { Id = currentUserId };
            var question = new Question { Id = Guid.NewGuid(), AuthorId = currentUserId };
            var questionDto = new QuestionDTO { Id = question.Id, AuthorId = currentUserId };
            var author = new User { Id = currentUserId, Username = "Author", PublicUsername = "PublicAuthor" };

            _currentUserProviderMock.Setup(x => x.GetCurrentUser())
                .Returns(currentUser);

            _unitOfWorkMock.Setup(x => x.QuestionRepository.GetQuestionsByDateAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { question }.AsQueryable());

            _mapperMock.Setup(x => x.Map<IEnumerable<QuestionDTO>>(It.IsAny<IEnumerable<Question>>()))
                .Returns(new[] { questionDto });

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(currentUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);

            var query = new GetPaginatedByDateQuestionsQuery { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Items.First().IsAuthor.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldCalculateTotalPagesCorrectly()
        {
            // Arrange
            var questions = Enumerable.Range(0, 25)
                .Select(i => new Question { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid() })
                .ToArray();

            _unitOfWorkMock.Setup(x => x.QuestionRepository.GetQuestionsByDateAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(questions.AsQueryable());

            _mapperMock.Setup(x => x.Map<IEnumerable<QuestionDTO>>(It.IsAny<IEnumerable<Question>>()))
                .Returns(questions.Select(q => new QuestionDTO { Id = q.Id, AuthorId = q.AuthorId }));

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User { Username = "Test", PublicUsername = "TestPublic" });

            var query = new GetPaginatedByDateQuestionsQuery { PageNumber = 2, PageSize = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.TotalPages.Should().Be(3); // 25 items / 10 per page = 3 pages
            result.CurrentPage.Should().Be(2);
        }
    }
}