using Microsoft.AspNetCore.SignalR;

namespace SignalRApp.API.Hubs
{
    public class MyHub : Hub
    {
        private static List<string> Messages { get; set; } = new List<string>();

        private static int ClientCount { get; set; } = 0;

        public static int TeamCount { get; set; } = 7;

        public async Task SendMessage(string message)
        {
            if (Messages.Count >= TeamCount)
            {
                await Clients.Caller.SendAsync("Error", $"Mesajlar {TeamCount} sayısı kadar sınırlandırılmıştır.");
            }
            else
            {
                Messages.Add(message);
                await Clients.All.SendAsync("RecieveMessage", message); 
            }
        }

        public async Task GetMessages()
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
