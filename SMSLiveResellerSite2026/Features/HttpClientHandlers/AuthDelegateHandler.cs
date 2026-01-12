using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace SMSLiveResellerSite2026.Features.HttpClientHandlers
{
    public class AuthDelegateHandler(AuthenticationStateProvider auth) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiKey = (await ((Authentication.SmsAuthProvider)auth).GetMember())?.ApiKey;

            if (apiKey != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
