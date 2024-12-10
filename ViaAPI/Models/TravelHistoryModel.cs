using System.ComponentModel.DataAnnotations;

namespace ViaAPI.Models
{
    public class TravelHistoryModel
    {
        [Key]
        public int Id { get; set; }
        public string NomeViagem { get; set; }
        public DateTime DataHoraFinalizacao { get; set; }
        public string UsuarioId { get; set; }
    }
}
