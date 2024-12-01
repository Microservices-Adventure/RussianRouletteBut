using Revolver.Domain.Config;
using Revolver.Domain.Models;

namespace Revolver.Domain.Services.Interfaces;

public interface IRevolverService
{
    ServiceInfo Roll(IReadOnlyList<ServiceInfo> services);
    Task<bool> Kill(ServiceInfo serviceInfo);
}