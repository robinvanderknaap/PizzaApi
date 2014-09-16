namespace Api.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public bool IsAuthenticated(string username, string password)
        {
            return username == "pizza" && password == "pizza";
        }
    }
}