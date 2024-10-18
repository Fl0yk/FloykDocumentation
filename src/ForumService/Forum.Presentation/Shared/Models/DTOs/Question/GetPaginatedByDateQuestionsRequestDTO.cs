namespace Forum.Presentation.Shared.Models.DTOs.Question;

public record class GetPaginatedByDateQuestionsRequestDTO(
                    int PageSize = 3,
                    int PageNumber = 1);
