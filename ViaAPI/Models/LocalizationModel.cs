using System.ComponentModel.DataAnnotations;

namespace ViaAPI.Models
{
    public class LocalizationModel
    {
        [Key]
        public Guid IdLocalizacao { get; set; } = Guid.NewGuid();
        public string NomeLinha { get; set; }
        public string DescricaoLocalizacao { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
