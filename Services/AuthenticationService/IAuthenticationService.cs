using System.Threading.Tasks;
using TodoClient.Models;
namespace TodoClient.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        AuthResponseDto User { get; }
        Task<AuthResponseDto> RegisterUser(RegisterModel userForRegistration);
        Task<AuthResponseDto> Login(AuthenticateModel userForAuthentication);
        Task<AuthResponseDto> CurrentUser();
        Task Initialize();
        Task Logout();
    }
}
