using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TodoClient.Services.AuthenticationService;

namespace TodoClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            //builder.Services.AddAuthorizationCore();
            //Add BaseAddress
            
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://todoapi.sofsog.com/api/") });
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            
            var host = builder.Build();
            var accountService = host.Services.GetRequiredService<IAuthenticationService>();
            await accountService.Initialize();
            await host.RunAsync();
        }
    }
}