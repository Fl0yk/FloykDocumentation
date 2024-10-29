namespace Identity.Application.Shared.Models.Requests.UserRequests;

public record class UpdateAvatarRequest
{
    public required string FileName { get; init; }

    public required Stream ImageStream { get; init; }
}

