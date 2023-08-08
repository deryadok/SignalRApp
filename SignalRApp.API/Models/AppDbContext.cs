using Microsoft.EntityFrameworkCore;

namespace SignalRApp.API.Models
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
    }
}
