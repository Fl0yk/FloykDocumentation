using Forum.Domain.Abstractions.Repositories;

namespace Forum.Infrastructure.BackgroundJobs.Question;

public class CloseQuestionsBackgroundJob
{
    private readonly IUnitOfWork _unitOfWork;

    public CloseQuestionsBackgroundJob(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CloseQuestionsAsync(int maxOpenedDays)
    {
        var questions = await _unitOfWork.QuestionRepository.GetOpenedQuestionsAsync();

        foreach (var question in questions)
        {
            if ((DateTime.UtcNow.Date - question.DateOfCreation).Days >= maxOpenedDays)
            {
                question.IsClosed = true;

                await _unitOfWork.QuestionRepository.UpdateQuestionAsync(question);
            }
        }

        await _unitOfWork.SaveChangesAsync(CancellationToken.None);
    }
}
