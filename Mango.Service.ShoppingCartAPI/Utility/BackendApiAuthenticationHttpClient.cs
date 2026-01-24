using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Mango.Service.ShoppingCartAPI.Utility;

public class BackendApiAuthenticationHttpClient(IHttpContextAccessor _httpContextAccessor):DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token");
        Console.WriteLine(token);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}
