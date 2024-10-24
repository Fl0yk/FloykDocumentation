namespace Identity.Application.Services.Requests.UserRequests;

public record class UpdateAvatarRequest(string FIleName, Stream ImageStream);
