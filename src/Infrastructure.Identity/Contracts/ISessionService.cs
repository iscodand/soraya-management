using SorayaManagement.Domain.Entities;

namespace SorayaManagement.Infrastructure.Identity.Contracts
{
    public interface ISessionService
    {
        public void AddUserSession(User user);
        public void RemoveUserSession();
        public User RetrieveUserSession();
    }
}