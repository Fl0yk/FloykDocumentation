using Hangfire.Dashboard;

namespace Forum.Presentation.Shared.Filters;

public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}