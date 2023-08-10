using SignalRApp.API.Models;

namespace SignalRApp.API.Hubs
{
    public interface IProductHub
    {
        Task RecieveProduct(Product p);
    }
}
