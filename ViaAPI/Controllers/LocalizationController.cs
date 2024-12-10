using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViaAPI.Data;
using ViaAPI.Jobs;
using ViaAPI.Services.LocalizationService;

namespace ViaAPI.Controllers
{
    [ApiController]
    [Route("Localization")]
    public class LocalizationController : ControllerBase
    {
        private readonly ViaJob _viaJob;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITravelHistoryService _travelHistoryService;
        private readonly ApiDbContext _dbContext;

        public LocalizationController(ViaJob viaJob, IHttpContextAccessor contextAccessor, ITravelHistoryService travelHistoryService, ApiDbContext apiDbContext)
        {
            _viaJob = viaJob;
            _httpContextAccessor = contextAccessor;
            _travelHistoryService = travelHistoryService;
            _dbContext = apiDbContext;
        }

        [HttpPost("start-atualization")]
        public async Task<IActionResult> IniciarAtualizacao(bool usarNovaRota = false)
        {
            var cancellationToken = new CancellationToken();
            await _viaJob.IniciarAtualizacao(cancellationToken, usarNovaRota);
            return Ok("Atualização iniciada.");
        }

        [HttpPost("stop-atualization")]
        public IActionResult PararAtualizacao()
        {
            _viaJob.StopAsync(new CancellationToken());
            return Ok("Atualização parada.");
        }

        [HttpPost("finalizar-viagem")]
        public async Task<IActionResult> FinalizarViagem()
        {
            await _viaJob.StopAsync(new CancellationToken());

            var userId = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

         
            string nomeViagem;

            switch (_viaJob._rotaAtual.First().NomeLinha)
            {
                case "ROTA 1":
                    nomeViagem = "Rota Jabaquara-Tucuruvi";
                    break;
                case "ROTA 2":
                    nomeViagem = "Rota Pedro II-Bresser Mooca";
                    break;
                default:
                    nomeViagem = "Rota Desconhecida";
                    break;
            }

            await _travelHistoryService.AddTravelHistory(userId, nomeViagem); 

            return Ok("Viagem finalizada e registrada no histórico.");
        }

        [HttpPost("alterar-rota")]
        public async Task<IActionResult> AlterarRota(bool usarNovaRota)
        {
            await _viaJob.ChangeRoute(usarNovaRota);
            return Ok("Rota alterada com sucesso.");
        }

        [HttpGet("historico-viagens")]
        public async Task<IActionResult> ObterHistoricoViagens()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Claims
                            .FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var historicoViagens = await _travelHistoryService.GetTravelHistory(userId);

            return Ok(historicoViagens);
        }


        [HttpGet("localizacoes")]
        public IActionResult ObterLocalizacoes()
        {
            var localizacoes = _viaJob.ObterLocalizacoes();
            return Ok(localizacoes);
        }
    }
}
