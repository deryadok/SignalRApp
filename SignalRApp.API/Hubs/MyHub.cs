using Microsoft.AspNetCore.SignalR;

namespace SignalRApp.API.Hubs
{
    public class MyHub : Hub
    {
        private static List<string> Messages { get; set; } = new List<string>();

        private static int ClientCount { get; set; } = 0;

        public async Task SendMessage(string message)
        {
            Messages.Add(message);
            await Clients.All.SendAsync("RecieveMessage", message);
        }

        public async Task GetMessage()
        {
            await Clients.All.SendAsync("RecieveMessages", Messages);
        }

        public override async Task OnConnectedAsync()
        {
            ClientCount++;
            await Clients.All.SendAsync("RecieveClientCount", ClientCount);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ClientCount--;
            await Clients.All.SendAsync("RecieveClientCount", ClientCount);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
