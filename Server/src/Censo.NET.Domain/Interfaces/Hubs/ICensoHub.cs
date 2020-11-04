using System.Threading.Tasks;

namespace Censo.NET.Domain.Interfaces.API
{
    public interface ICensoHub
    {
        Task SendMessage(object message);
    }
}
