namespace Api.Infrastructure.Services
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated(string username, string password);
    }
}
