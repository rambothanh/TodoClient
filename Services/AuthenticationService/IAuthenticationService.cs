using System.Threading.Tasks;
using TodoClient.Models;
namespace TodoClient.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> RegisterUser(RegisterModel userForRegistration);
        Task<AuthResponseDto> Login(AuthenticateModel userForAuthentication);
        //Task Logout();
    }
}
