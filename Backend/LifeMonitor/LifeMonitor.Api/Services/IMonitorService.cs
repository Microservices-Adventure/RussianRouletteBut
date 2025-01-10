using LifeMonitor.Api.Models;

namespace LifeMonitor.Api.Services
{
    public interface IMonitorService
    {
        Task<LifeServiceModel> GetLife(string serviceHost, string port);
    }
}
