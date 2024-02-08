using Domain.Entities;

namespace Application.Contracts.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        public Task<Role> GetRoleByIdAsync(string roleId);
    }
}