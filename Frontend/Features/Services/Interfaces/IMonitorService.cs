using Frontend.Entities.LifeMonitor.Model;

namespace Frontend.Features.Services.Interfaces;

public interface IMonitorService
{
    Task<IReadOnlyCollection<LifeServiceModel>> GetLifes();
}