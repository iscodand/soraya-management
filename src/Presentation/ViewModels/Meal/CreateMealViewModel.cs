using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class CreateMealViewModel
    {
        [Required(ErrorMessage = "O nome do sabor é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O sabor precisa ter no mínimo 3 caracteres e no máximo 50.")]
        public string Description { get; set; }
        public string Accompaniments { get; set; }
    }
}