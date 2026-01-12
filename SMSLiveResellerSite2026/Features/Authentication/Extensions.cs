using Microsoft.AspNetCore.Components.Authorization;

namespace SMSLiveResellerSite2026.Features.Authentication
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddSmsAuthProvider(this IServiceCollection services)
        {
            return services.AddScoped<AuthenticationStateProvider, SmsAuthProvider>();
        }
    }
}
