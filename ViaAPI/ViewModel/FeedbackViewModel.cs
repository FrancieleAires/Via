using System.ComponentModel.DataAnnotations;

public class FeedbackViewModel
{

    [Required(ErrorMessage = "O comentário é obrigatório.")]
    public string Comentario { get; set; }

    [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5.")]
    public int Nota { get; set; }

    public DateTime Data { get; set; }
}