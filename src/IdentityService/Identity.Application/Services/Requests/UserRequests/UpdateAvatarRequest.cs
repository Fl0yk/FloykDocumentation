namespace Identity.Application.Services.Requests.UserRequests;

public record class UpdateAvatarRequest(string FileName, Stream ImageStream);
