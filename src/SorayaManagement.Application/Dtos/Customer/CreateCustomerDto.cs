using System.ComponentModel.DataAnnotations;

namespace SorayaManagement.Application.Dtos.Customer
{
    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
    }
}