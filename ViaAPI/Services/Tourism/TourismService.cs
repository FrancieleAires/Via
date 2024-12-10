using ViaAPI.Models;

namespace ViaAPI.Services.TourismService
{
    public class TourismService : ITourismService
    {
        private readonly List<TourismModel> _tourism = new List<TourismModel>
        {
            new TourismModel { NomePontoTuristico = "Beco do Robin", Latitude = -23.646736, Longitude = -46.640434 },
            new TourismModel { NomePontoTuristico = "Centro Cultural SP", Latitude = -23.646328, Longitude = -46.640772 }

            //Mudei as latitudes e longitudes para teste, para ver como tá retornando na notificação. 

             /* new TourismModel { NomePontoTuristico = "Beco do Robin", Latitude = -23.645441, Longitude = -46.641206 },
            new TourismModel { NomePontoTuristico = "Centro Cultural SP", Latitude = -23.570797527297525, Longitude = -46.640059732450425 }*/
        };

        public List<TourismModel> GetAllTourism()
        {
            return _tourism;
        }

        public List<TourismModel> SearchTourism(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<TourismModel>();
            }

            return _tourism
                .Where(t => t.NomePontoTuristico.Contains(query, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
