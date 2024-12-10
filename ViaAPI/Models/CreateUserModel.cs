using System.ComponentModel.DataAnnotations;
namespace ViaAPI.Models
{
    public class CreateUserModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string CPF { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public DateTime dataNascimento { get; set; }
        public required string Telefone { get; set; }
    }
}
