using SMSLiveResellerSite2026.Services;
using SMSLiveResellerSite2026.Features.HttpClientHandlers;
using SMSLiveResellerSite2026.Features.Authentication;
using Blazored.LocalStorage;
using SMSLive247.OpenApi;
using SMSLiveResellerSite2026.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using SMSLiveResellerSite2026.Components;

namespace SMSLiveResellerSite2026
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var settings = new Settings();
            builder.Configuration.Bind(settings);
            builder.Services.AddSingleton(settings);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Register foundational services first so others can depend on them
            builder.Services.AddBlazoredLocalStorage(); // ILocalStorageService is scoped
            builder.Services.AddMemoryCache(); // IMemoryCache for cache handler

            // Authentication - depends on ILocalStorageService
            builder.Services.AddSmsAuthProvider(); // registers AuthenticationStateProvider (scoped)

            // Register authentication services required by server-side components
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie();

            builder.Services.AddAuthorizationCore();

            // Application services
            builder.Services.AddSingleton<SpinnerService>();
            builder.Services.AddSingleton<AlertService>();
            builder.Services.AddTransient<StorageService>();

            // HttpClient handlers - transient is appropriate for handlers
            builder.Services.AddTransient<AuthDelegateHandler>();
            builder.Services.AddTransient<SpinnerDelegateHandler>();
            builder.Services.AddTransient<CacheDelegateHandler>();

            // Configure typed API clients. Order handlers so Cache runs first (can short-circuit),
            // then Spinner (show while performing request), then Auth (adds header before sending).
            builder.Services.AddHttpClient<ApiClient>(ConfigureUrl)
                            .AddHttpMessageHandler<CacheDelegateHandler>()
                            .AddHttpMessageHandler<AuthDelegateHandler>();

            builder.Services.AddHttpClient<SubAccountClient>(ConfigureUrl);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            // Enable authentication/authorization middleware for server-side Blazor
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();

            void ConfigureUrl(HttpClient client)
            {
                client.BaseAddress = new Uri(settings.BaseUrl);
            }
        }
    }
}
