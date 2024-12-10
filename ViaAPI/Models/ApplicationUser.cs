using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ViaAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres")]
        public string Nome { get; set; }


        [Required(ErrorMessage = "O campo {0} não é uma data válida")]
        [DataType(DataType.Date, ErrorMessage = "O campo {0} não é uma data válida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression(@"\d{3}\.\d{3}\.\d{3}-\d{2}", ErrorMessage = "O CPF deve estar no formato 000.000.000-00")]
        public string CPF { get; set; }









    }
}
