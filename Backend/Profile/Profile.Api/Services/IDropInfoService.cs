using Profile.Api.DataAccess.Entity;
using Profile.Api.Models;

namespace Profile.Api.Services
{
    public interface IDropInfoService
    {
        Task<UserProfile> AddUserProfileAsync(AddUserProfileRequest request);
        Task<UserProfile> GetUserProfileAsync(GetUserProfileRequest request);
        Task<DropInfo> AddDropInfoByUsernameAsync(AddDropInfoByUsernameRequest request, CancellationToken ct);
    }
}
