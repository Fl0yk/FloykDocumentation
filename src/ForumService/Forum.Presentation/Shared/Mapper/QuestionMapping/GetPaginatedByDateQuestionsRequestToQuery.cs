using AutoMapper;
using Forum.Application.UseCase.Query.Question;
using Forum.Presentation.Shared.Models.DTOs.Question;

namespace Forum.Presentation.Shared.Mapper.QuestionMapping;

public class GetPaginatedByDateQuestionsRequestToQuery : Profile
{
    public GetPaginatedByDateQuestionsRequestToQuery()
    {
        CreateMap<GetPaginatedByDateQuestionsRequestDTO, GetPaginatedByDateQuestionsQuery>()
            .ForMember(d => d.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(d => d.PageSize, opt => opt.MapFrom(src => src.PageSize));
    }
}
