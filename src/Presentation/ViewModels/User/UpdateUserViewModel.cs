namespace Presentation.ViewModels.User
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string NewUsername { get; set; }
        public string NewEmail { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
    }
}