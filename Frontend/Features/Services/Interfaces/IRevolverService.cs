using System.Net;
using Frontend.Entities.Revolver.Model;

namespace Frontend.Features.Interfaces;

public interface IRevolverService
{
    Task<KilledServiceInfo> Shoot();
}