using Application.Dtos.User;
using Application.DTOs.Authentication;

namespace Application.Contracts.Services
{
    public interface ISessionService
    {
        public void AddUserSession(GetAuthenticatedUserDto user);
        public void RemoveUserSession();
        public GetAuthenticatedUserDto RetrieveUserSession();
    }
}