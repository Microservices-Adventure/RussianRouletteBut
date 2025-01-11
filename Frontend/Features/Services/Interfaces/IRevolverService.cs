using System.Net;
using Frontend.Entities.Revolver.Model;

namespace Frontend.Features.Services.Interfaces;

public interface IRevolverService
{
    Task<KilledServiceInfo> Shoot(ShootMan shootMan);
}