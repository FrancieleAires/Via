using System.ComponentModel.DataAnnotations;

namespace ViaAPI.ViewModel
{
    public class RegisterUserViewModel
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

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression(@"\d{3}\.\d{3}\.\d{3}-\d{2}", ErrorMessage = "O CPF deve estar no formato 000.000.000-00")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo {0} não é um número de telefone válido")]
        [RegularExpression(@"^\(\d{2}\)\d{4,5}-\d{4}$", ErrorMessage = "O número de telefone deve estar no formato (XX)XXXX-XXXX ou (XX)XXXXX-XXXX")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmarSenha { get; set; }


    }
}
