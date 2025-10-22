using Logs.WebUI.Components;
using Logs.WebUI.Data;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Logs.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddFluentUIComponents();

            // Configure session state
            builder.Services.AddSingleton<SessionState>(
                new SessionState() {
                    CurrentUser = null,
                    AccessToken = null
                }
            );

            // Configure API client
            var url = "http://localhost:5194";
            builder.Services.AddSingleton<Client>(
                new Client(
                    url,
                    new HttpClient()
                    {
                        BaseAddress = new Uri(url)
                    }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
