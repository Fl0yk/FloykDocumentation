using Microsoft.AspNetCore.SignalR;

namespace Forum.Infrastructure.SignalR.Hubs;

public class QuestionsHub : Hub
{
    public async Task JoinInGroup(Guid questionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, questionId.ToString());
    }

    public async Task LeaveFromGroup(Guid questionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, questionId.ToString());
    }
}
