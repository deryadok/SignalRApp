using Microsoft.AspNetCore.SignalR;

namespace SignalRApp.API.Hubs
{
    public class MyHub : Hub
    {
        public static List<string> Messages { get; set; } = new List<string>();

        public async Task SendMessage(string message)
        {
            Messages.Add(message);
            await Clients.All.SendAsync("RecieveMessage", message);
        }

        public async Task GetMessage()
        {
            await Clients.All.SendAsync("RecieveMessages", Messages);
        }
    }
}
