using MassTransit;

namespace Article.Contracts.Events.User;

[MessageUrn("identity")]
[EntityName("username-updated")]
public interface UsernameUpdated
{
    public string OldUsername { get; set; }

    public string NewUsername { get; set; }
}
