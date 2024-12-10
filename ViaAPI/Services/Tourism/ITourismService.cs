using ViaAPI.Models;

namespace ViaAPI.Services.TourismService
{
    public interface ITourismService
    {

        public List<TourismModel> GetAllTourism();
        public List<TourismModel> SearchTourism(string query);

    }
}
