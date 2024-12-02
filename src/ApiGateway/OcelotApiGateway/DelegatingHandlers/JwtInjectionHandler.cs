using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OcelotApiGateway.DelegatingHandlers;

public class JwtInjectionHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _contextAccessor;

    public JwtInjectionHandler(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

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

        if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(currentUserName))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        if (request.Method == HttpMethod.Post || request.Method == HttpMethod.Put 
            || request.Method == HttpMethod.Patch || request.Method == HttpMethod.Delete)
        {
            if (request.Content is not null && request.Content.Headers.ContentType?.MediaType == "multipart/form-data")
            {
                await MultipartDataHandling(request, currentUserName, currentUserId);
            }
            else
            {
                await JsonDataHandling(request, currentUserName, currentUserId);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task MultipartDataHandling(HttpRequestMessage request, string currentUserName, string currentUserId)
    {
        var httpContext = _contextAccessor.HttpContext;

        var newContent = new MultipartFormDataContent();

        if (httpContext!.Request.Form != null && httpContext.Request.Form.Files != null)
        {
            foreach (var f in httpContext.Request.Form.Files)
            {
                using (var memStream = new MemoryStream())
                {
                    await f.CopyToAsync(memStream);

                    var fileContent = new ByteArrayContent(memStream.ToArray());

                    newContent.Add(fileContent, f.Name, f.FileName);
                }
            }
        }

        if (httpContext.Request.Form != null)
        {
            foreach (var key in httpContext.Request.Form.Keys)
            {
                var strContent = new StringContent(httpContext!.Request!.Form[key]!);

                newContent.Add(strContent, key);
            }
        }

        newContent.Add(new StringContent(currentUserName), "currentUserName");
        newContent.Add(new StringContent(currentUserId), "currentUserId");

        request.Content = newContent;
    }

    private async Task JsonDataHandling(HttpRequestMessage request, string currentUserName, string currentUserId)
    {
        var content = await request.Content!.ReadAsStringAsync();

        var json = string.IsNullOrEmpty(content) ? new JObject() : JObject.Parse(content);

        json["currentUserName"] = currentUserName;
        json["currentUserId"] = currentUserId;

        request.Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
    }
}