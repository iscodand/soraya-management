using SorayaManagement.Domain.Entities;

namespace SorayaManagement.Infrastructure.Identity.Contracts
{
    public interface IAuthenticatedUserService
    {
        public Task<User> GetAuthenticatedUser();
    }
}