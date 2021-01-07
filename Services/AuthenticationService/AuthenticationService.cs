using System.Threading.Tasks;
using TodoClient.Models;
namespace TodoClient.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<AuthResponseDto> Login(AuthenticateModel userForAuthentication)
        {
            throw new System.NotImplementedException();
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
