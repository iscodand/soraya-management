using Domain.Entities;

namespace Infrastructure.Data.Contracts
{
    public interface IUserRepository
    {
        public Task<User> GetUserByUsernameAsync(string username);
        public Task<ICollection<User>> GetUsersByCompanyAsync(int companyId);
    }
}