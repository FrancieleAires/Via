using Microsoft.EntityFrameworkCore;
using ViaAPI.Data;
using ViaAPI.Models;
using ViaAPI.Services.LocalizationService;

public class TravelHistoryService : ITravelHistoryService
{
    private readonly ApiDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TravelHistoryService(ApiDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TravelHistoryModel> AddTravelHistory(string userId, string rotaAtual)
    {
        var viagem = new TravelHistoryModel
        {
            NomeViagem = ObterNomeViagem(rotaAtual),
            DataHoraFinalizacao = DateTime.Now,
            UsuarioId = userId
        };

        await _dbContext.TravelHistory.AddAsync(viagem); 
        await _dbContext.SaveChangesAsync(); 

        return viagem; 
    }
    public async Task<List<TravelHistoryModel>> GetTravelHistory(string userId)
    {
        return await _dbContext.TravelHistory
            .Where(v => v.UsuarioId == userId)
            .ToListAsync();
    }
    public string ObterNomeViagem(string rotaAtual)
    {
        if (rotaAtual == "Rota Jabaquara-Tucuruvi")
        {
            return "Rota Jabaquara-Tucuruvi";
        }
        else if (rotaAtual == "Rota Pedro II-Bresser Mooca")
        {
            return "Rota Pedro II-Bresser Mooca";
        }
        else
        {
            return "Rota Desconhecida"; 
        }
    }
}
