using AutoMapper;
using Core.Exceptions;
using FluentAssertions;
using Forum.Application.Shared.Models.DTOs;
using Forum.Application.UseCase.Query.Answer;
using Forum.Domain.Abstractions.Repositories;
using Forum.Domain.Entities;
using Moq;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Forum.Application.UseCase.QueryHandlers.Answer;

namespace Forum.Application.UnitTests.UseCase.QueryHandlers.Answers
{
    public class GetAnswerByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAnswerByIdQueryHandler _handler;

        public GetAnswerByIdQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAnswerByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenAnswerNotFound_ShouldThrowGuardNotFoundException()
        {
            // Arrange
            var answerId = Guid.NewGuid();

            _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdAsync(answerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Answer)null);

            var query = new GetAnswerByIdQuery { Id = answerId };

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<GuardNotFoundException>()
                .WithMessage($"Answer with id {answerId} not found");
        }

        [Fact]
        public async Task Handle_WhenAnswerExists_ShouldReturnMappedAnswerDto()
        {
            // Arrange
            var answerId = Guid.NewGuid();
            var answer = new Answer { Id = answerId };
            var expectedDto = new AnswerDTO { Id = answerId };

            _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdAsync(answerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(answer);

            _mapperMock.Setup(x => x.Map<AnswerDTO>(answer))
                .Returns(expectedDto);

            var query = new GetAnswerByIdQuery { Id = answerId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedDto);
            _unitOfWorkMock.Verify(x => x.AnswerRepository.FirstOrDefaultByIdAsync(
                answerId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(x => x.Map<AnswerDTO>(answer), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldPassCorrectCancellationToken()
        {
            // Arrange
            var answerId = Guid.NewGuid();
            var cancellationToken = new CancellationToken(true);
            var answer = new Answer { Id = answerId };
            var expectedDto = new AnswerDTO { Id = answerId };

            _unitOfWorkMock.Setup(x => x.AnswerRepository.FirstOrDefaultByIdAsync(answerId, cancellationToken))
                .ReturnsAsync(answer);

            _mapperMock.Setup(x => x.Map<AnswerDTO>(answer))
                .Returns(expectedDto);

            var query = new GetAnswerByIdQuery { Id = answerId };

            // Act
            var result = await _handler.Handle(query, cancellationToken);

            // Assert
            result.Should().BeEquivalentTo(expectedDto);
            _unitOfWorkMock.Verify(x => x.AnswerRepository.FirstOrDefaultByIdAsync(
                answerId, cancellationToken), Times.Once);
        }
    }
}