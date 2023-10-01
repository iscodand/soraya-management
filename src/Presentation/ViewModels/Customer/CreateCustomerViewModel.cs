using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels.Customer
{
    public class CreateCustomerViewModel
    {
        [Required(ErrorMessage = "O nome do cliente é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Seu nome precisa ter no mínimo 3 caracteres e no máximo 50.")]
        public string Name { get; set; }

        [StringLength(17, MinimumLength = 17, ErrorMessage = "O número de telefone precisa ter exatamente os 11 dígitos.")]
        public string Phone { get; set; }
    }
}