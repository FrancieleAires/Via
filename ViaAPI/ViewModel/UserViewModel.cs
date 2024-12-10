using System.ComponentModel.DataAnnotations;

public class UserViewModel
{
    [Required]
    public string Nome { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    [DataType(DataType.Date)]
    public DateTime DataNascimento { get; set; }

    [Required]
    public string CPF { get; set; }
}