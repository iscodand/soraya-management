using System.ComponentModel.DataAnnotations;

namespace SorayaManagement.ViewModels
{
    public class CreateCustomerViewModel
    {
        [Required(ErrorMessage = "O nome do cliente é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Seu nome precisa ter no mínimo 3 caracteres e no máximo 50.")]
        public string Name { get; set; }
    }
}