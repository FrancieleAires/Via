using System.ComponentModel.DataAnnotations;

namespace ViaAPI.Models
{
    public class FeedbackModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [StringLength(500)]
        public string Comentario { get; set; }
        public int Avaliacao { get; set; }


        public DateTime Data { get; set; }

    }
}
