using System.Net;

namespace Frontend.Features.Interfaces;

public interface IRevolverService
{
    Task<bool> Shoot();
}