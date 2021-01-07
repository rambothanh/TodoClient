using System.Threading.Tasks;
using TodoClient.Models;
using System.Net.Http.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Text.Json;
using System.Text;

namespace TodoClient.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient Http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        public AuthenticationService(HttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            Http = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }
        public async Task<AuthResponseDto> Login(AuthenticateModel userForAuthentication)
        {
            var respone = await Http.PostAsJsonAsync("/api/Users/authenticate", userForAuthentication);
            // var content = JsonSerializer.Serialize(userForAuthentication);
            // var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            // var authResult = await _client.PostAsync("/", bodyContent);
            var authContent = await respone.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (!respone.IsSuccessStatusCode)
                return result;
            await _localStorage.SetItemAsync("authToken", result.Token);
            //((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(userForAuthentication.Username;
            Http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", result.Token);
            return new AuthResponseDto { IsAuthSuccessful = true };
        }

        public Task Logout()
        {
            throw new System.NotImplementedException();
        }

        public Task<RegisterModel> RegisterUser(RegisterModel userForRegistration)
        {
            throw new System.NotImplementedException();
        }
    }
}
