using Application.Dtos.User;
using Domain.Entities;

namespace Infrastructure.Identity.Contracts
{
    public interface ISessionService
    {
        public void AddUserSession(GetAuthenticatedUserDto user);
        public void RemoveUserSession();
        public GetAuthenticatedUserDto RetrieveUserSession();
    }
}