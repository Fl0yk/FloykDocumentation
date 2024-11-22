using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OcelotApiGateway.DelegatingHandlers;

public class JwtInjectionHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization?.Scheme != "Bearer")
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var token = request.Headers.Authorization.Parameter;
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var jwtToken = handler.ReadJwtToken(token);

        var currentUserName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var currentUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (String.IsNullOrEmpty(currentUserId) || String.IsNullOrEmpty(currentUserName))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put 
            || request.Method == HttpMethod.Patch || request.Method == HttpMethod.Delete)
        {
            var content = await request.Content!.ReadAsStringAsync();

            var json = string.IsNullOrEmpty(content) ? new JObject() : JObject.Parse(content);

            json["currentUserName"] = currentUserName;
            json["currentUserId"] = currentUserId;

            request.Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}