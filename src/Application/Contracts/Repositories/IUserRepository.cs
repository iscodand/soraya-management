using Domain.Entities;

namespace Application.Contracts.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User> GetUserByUsernameAsync(string username);
        public Task<ICollection<User>> GetUsersByCompanyAsync(int companyId);
        public Task<bool> UserExistsByUsernameAsync(string username);
        public Task<bool> UserExistsByEmailAsync(string email);
    }
}