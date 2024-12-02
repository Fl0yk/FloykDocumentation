using Article.Application.UseCases.Requests.Articles;
using Article.Contracts.Events.User;
using AutoMapper;

namespace Article.Infrastructure.Shared.Mappers.Article;

public class UsernameUpdatedContextToRequest : Profile
{
    public UsernameUpdatedContextToRequest()
    {
        CreateMap<UsernameUpdated, UpdateUsernameForAuthorsRequest>()
            .ForMember(d => d.NewUsername, opt => opt.MapFrom(src => src.NewUsername))
            .ForMember(d => d.OldUsername, opt => opt.MapFrom(src => src.OldUsername));
    }
}
