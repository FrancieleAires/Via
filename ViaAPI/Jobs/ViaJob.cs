
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using ViaAPI.Data;
using ViaAPI.Hubs;
using ViaAPI.Models;
using ViaAPI.Rotas;
using ViaAPI.Services.LocalizationService;
using ViaAPI.Services.TourismService;

namespace ViaAPI.Jobs
{
    public class ViaJob : IHostedService
    {
        private Timer _timer;
        private bool _isUpdating = false;
        public IServiceProvider Services { get; }
        private int _currentIndex = 0;
        private readonly ITourismService _tourismService;
        private readonly IHubContext<LocalizationHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public List<LocalizationModel> _localizacoes;
        private List<TourismModel> _pontosTuristicos;
        private List<LocalizationModel> _novaLocalizacoes;
        public List<LocalizationModel> _rotaAtual;


        public ViaJob(IServiceProvider services, ITourismService _tourismService, IHubContext<LocalizationHub> hubContext, IHttpContextAccessor httpContextAccessor, IServiceScopeFactory serviceScopeFactory)
        {
            Services = services;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            _serviceScopeFactory = serviceScopeFactory;

            _localizacoes = Rota.Rota1;
            _novaLocalizacoes = Rota.Rota2;
            _pontosTuristicos = _tourismService.GetAllTourism();
            _rotaAtual = _localizacoes;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public async Task IniciarAtualizacao(CancellationToken cancellationToken, bool usarNovaRota = false)
        {
            //ao invés de usar o if, preferi usar assim (segue abaixo), economiza linha  :)
            _rotaAtual = usarNovaRota ? _novaLocalizacoes : _localizacoes;
            if (!_isUpdating)
            {
                _isUpdating = true;
                _currentIndex = 0; 
                _timer = new Timer(_ => AtualizarLocalizacao(null, cancellationToken), null, 0, 3000);
            }
        }

        public async Task AtualizarLocalizacao(object state, CancellationToken cancellationToken)
        {
            if (!_isUpdating) return;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
                var travelHistoryService = scope.ServiceProvider.GetRequiredService<ITravelHistoryService>();
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<LocalizationHub>>();

                if (_currentIndex < _rotaAtual.Count)
                {
                    var local = _rotaAtual[_currentIndex];
                    Console.WriteLine($"Latitude: {local.Latitude} - Longitude: {local.Longitude}");
                    await _hubContext.Clients.All.SendAsync("ReceiveLocalization", JsonSerializer.Serialize(local));

                    foreach (var ponto in _pontosTuristicos)
                    {
                        if (IsNearTouristSpot(local, ponto))
                        {
                            Console.WriteLine($"Ponto turístico próximo: ", ponto);
                            await _hubContext.Clients.All.SendAsync("ReceiveLocalization", ("Ponto turístico próximo: " + ponto));
                        }
                    }
                    _currentIndex++;
                }
                else
                {
                    string mensagemCompleta = "Você chegou ao seu destino, rota finalizada!";
                    await hubContext.Clients.All.SendAsync("ReceiveLocalization", mensagemCompleta);
                    string nomeViagem;

                    switch (_rotaAtual.First().NomeLinha)
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

                    await SaveTravelHistory(scope, dbContext, nomeViagem);
                    await hubContext.Clients.All.SendAsync("RouteCompleted");

                    ReiniciarRota();

                }
            }
        }
        public async Task ChangeRoute(bool usarNovaRota)
        {
            _rotaAtual = usarNovaRota ? _novaLocalizacoes : _localizacoes;
            _currentIndex = 0; 

            if (!_isUpdating)
            {
                _isUpdating = true;
                _timer = new Timer(_ => AtualizarLocalizacao(null, CancellationToken.None), null, 0, 3000);
            }

            await _hubContext.Clients.All.SendAsync("ReceiveLocalization", $"Mudando para a {(usarNovaRota ? "nova" : "rota original")}!");
        }
        private void ReiniciarRota()
        {
            _currentIndex = 0;
        }
        public async Task SaveTravelHistory(IServiceScope scope, ApiDbContext dbContext, string nomeViagem)
        {
            var userId = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.User?.Claims.FirstOrDefault
                (c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))?.Value;


            if (!string.IsNullOrEmpty(userId))
            {
                var viagem = new TravelHistoryModel
                {
                    NomeViagem = nomeViagem,
                    DataHoraFinalizacao = DateTime.Now,
                    UsuarioId = userId
                };

                dbContext.TravelHistory.Add(viagem);
                await dbContext.SaveChangesAsync();
            }
        }
        public bool IsNearTouristSpot(LocalizationModel localizationModel, TourismModel tourismModel)
        {
            const double tourismNotificationRadius = 0.00045045045;

            double distance = Math.Sqrt(
                Math.Pow(localizationModel.Latitude - tourismModel.Latitude, 2) +
                Math.Pow(localizationModel.Longitude - tourismModel.Longitude, 2)
            );

            return distance < tourismNotificationRadius;
        }
        public List<LocalizationModel> ObterLocalizacoes()
        {
            return _rotaAtual;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _isUpdating = false;
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
