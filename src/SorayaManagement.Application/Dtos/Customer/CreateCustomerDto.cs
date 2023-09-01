using System.ComponentModel.DataAnnotations;

namespace SorayaManagement.Application.Dtos.Customer
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "O nome do cliente é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Seu nome precisa ter no mínimo 3 caracteres e no máximo 50.")]
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public int CompanyId { get; set; }
    }
}