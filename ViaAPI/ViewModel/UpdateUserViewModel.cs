using System.ComponentModel.DataAnnotations;

namespace ViaAPI.ViewModel
{
    public class UpdateUserViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} não é uma data válida")]
        [DataType(DataType.Date, ErrorMessage = "O campo {0} não é uma data válida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo {0} não é um número de telefone válido")]
        [RegularExpression(@"^\(\d{2}\)\d{4,5}-\d{4}$", ErrorMessage = "O número de telefone deve estar no formato (XX)XXXX-XXXX ou (XX)XXXXX-XXXX")]
        public string PhoneNumber { get; set; }
    }
}
