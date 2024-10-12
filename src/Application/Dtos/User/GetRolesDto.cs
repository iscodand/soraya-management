using Domain.Entities;

namespace Application.Dtos.User
{
    public class GetRolesDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static IEnumerable<GetRolesDto> Map(IEnumerable<Role> roles)
        {
            return roles.Select(x => new GetRolesDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            });
        }
    }
}