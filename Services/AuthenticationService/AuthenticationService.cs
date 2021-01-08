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
using Microsoft.AspNetCore.Components;

namespace TodoClient.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient Http;
        //private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private NavigationManager _navi;
        private string userKeyInLocal ="user";
        public AuthResponseDto User { get; private set; }

        public AuthenticationService(HttpClient client, 
        ILocalStorageService localStorage,
        NavigationManager navi)
        {
            Http = client;
            //_authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _navi = navi;
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
            result.IsAuthSuccessful = true;
            //Lưu trữ Token và toàn bộ đối tượng User vào vào localStorage
            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync(userKeyInLocal, result);
            
            //Thêm xác nhận Token vào Header mặc định của Http 
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            //Trả về Oject AuthResponseDto chứa xác nhận thành công
            return result;
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
        public async Task<AuthResponseDto> CurrentUser(){
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if(string.IsNullOrWhiteSpace(token))
                return new AuthResponseDto{IsAuthSuccessful=false};
            var result = await _localStorage.GetItemAsync<AuthResponseDto>(userKeyInLocal);
            return result;
        }
        public async Task Initialize()
        {
            User = await _localStorage.GetItemAsync<AuthResponseDto>(userKeyInLocal);
            
        }

        public async Task Logout()
        {
            User = null;
            await _localStorage.RemoveItemAsync(userKeyInLocal);
            _navi.NavigateTo("signin");
        }

    }
}
