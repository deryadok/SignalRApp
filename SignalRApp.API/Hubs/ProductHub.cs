using Microsoft.AspNetCore.SignalR;
using SignalRApp.API.Models;

namespace SignalRApp.API.Hubs
{
    public class ProductHub : Hub<IProductHub>
    {
        public async Task SendProduct(Product p)
        {
            await Clients.All.RecieveProduct(p);
        }
    }
}
