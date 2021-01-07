using System.Threading.Tasks;
using TodoClient.Models;
using System.Net.Http.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Text.Json;
using System.Text;
using TodoClient.Helpers;
using System.Net.Http.Headers;

namespace TodoClient.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient Http;
        //private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        public AuthenticationService(HttpClient client, ILocalStorageService localStorage)
        {
            Http = client;
            //_authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthResponseDto> Login(AuthenticateModel userForAuthentication)
        {
            var respone = await Http.PostAsJsonAsync("/api/Users/authenticate", userForAuthentication);
            var authContent = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            //Sử dụng StringConverter tự tạo thay cho JsonConverter để tránh bị lỗi
            //chuyển Json thành Oject: 
            options.Converters.Add(new StringConverter());
            var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, options);
            //Nếu không phải phản hồi thành công thì trả về result
            //trong đó chỉ chứa Message báo lỗi và IsAuthSuccessful=false
            if (!respone.IsSuccessStatusCode)
                return result;
            //Lưu trữ Token vào localStorage
            await _localStorage.SetItemAsync("authToken", result.Token);
            //Thêm xác nhận Token vào Header mặc định của Http 
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            //Trả về Oject AuthResponseDto chứa xác nhận thành công
            return new AuthResponseDto { IsAuthSuccessful = true };
            //
            //((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(userForAuthentication.Email);
        }

        public async Task<AuthResponseDto> RegisterUser(RegisterModel userForRegistration)
        {
            var respone = await Http.PostAsJsonAsync("/api/Users/register", userForRegistration);
            if (!respone.IsSuccessStatusCode)
                return new AuthResponseDto { Message = $"Username {userForRegistration.Username} is already taken" };
            
            return new AuthResponseDto { IsAuthSuccessful = true };
        }
    }
}
