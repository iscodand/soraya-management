using Domain.Entities;

namespace Infrastructure.Data.Contracts
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        public Task<Role> GetRoleByIdAsync(string roleId);
    }
}