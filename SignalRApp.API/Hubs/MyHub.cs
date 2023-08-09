using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRApp.API.Models;

namespace SignalRApp.API.Hubs
{
    public class MyHub : Hub
    {
        private readonly AppDbContext _context;

        public MyHub(AppDbContext context)
        {
            _context = context;
        }

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

        //Groups
        public async Task AddGroup(string teamName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamName);
        }

        public async Task RemoveFromGroup(string teamName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, teamName);
        }

        public async Task SendNameByGroup(string name, string teamName)
        {
            var team = _context.Teams.Where(x => x.Name == teamName).FirstOrDefault();

            if (team != null)
            {
                team.Users.Add(new User() { Name = name });
            }
            else
            {
                var newTeam = new Team() { Name = teamName };

                newTeam.Users.Add(new User() { Name = name });

                _context.Teams.Add(newTeam);
            }

            await _context.SaveChangesAsync();

            await Clients.Group(teamName).SendAsync("RecieveMessageByGroup", name, team.Id);
        }

        public async Task GetNamesByGroup()
        {
            var teams = _context.Teams.Include(x => x.Users).Select(x => new 
            {
                teamId = x.Id,
                users = x.Users.ToList()
            }).ToList();

            await Clients.All.SendAsync("RecieveNamesByGroup", teams);
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
