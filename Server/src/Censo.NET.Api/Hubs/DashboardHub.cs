using Censo.NET.Domain.Interfaces.API;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Censo.NET.API.Hubs
{
    public class DashboardHub : Hub, ICensoHub
    {
        private readonly IHubContext<DashboardHub> _context;

        public DashboardHub(IHubContext<DashboardHub> context)
        {
            _context = context;
        }

        public async Task SendMessage(object message) =>
            await _context.Clients?.All.SendAsync("AtualizarDashboard", message);
    }
}
