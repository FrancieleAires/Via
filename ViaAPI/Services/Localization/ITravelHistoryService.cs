using ViaAPI.Models;

namespace ViaAPI.Services.LocalizationService
{
    public interface ITravelHistoryService
    {
        Task<TravelHistoryModel>AddTravelHistory(string userId, string nomeViagem);
        public string ObterNomeViagem(string rotaAtual);
        Task<List<TravelHistoryModel>> GetTravelHistory(string userId);
    }
}
