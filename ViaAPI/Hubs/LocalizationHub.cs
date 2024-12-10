


using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace ViaAPI.Hubs
{
    public sealed class LocalizationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveLocalization", "teste mensagem");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine(exception);
        }
    }
}
