using System.Net.Http.Headers;

namespace HealthApp.AdminBlazor.Services;

public class AdminAuthorizationMessageHandler(
    TokenStorageService tokenStorageService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await tokenStorageService.GetAccessTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}