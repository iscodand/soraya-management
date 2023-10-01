using Domain.Entities;

namespace Infrastructure.Identity.Contracts
{
    public interface ISessionService
    {
        public void AddUserSession(User user);
        public void RemoveUserSession();
        public User RetrieveUserSession();
    }
}