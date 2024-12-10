using System.ComponentModel.DataAnnotations;

namespace ViaAPI.Models
{
    public class TourismModel
    {
        [Key]
        public Guid IdTurismo { get; set; } = Guid.NewGuid();
        public string NomePontoTuristico { get; set; }
        public string DescricaoLocal { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}
