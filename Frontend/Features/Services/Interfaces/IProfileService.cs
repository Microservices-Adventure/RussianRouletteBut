using Frontend.Entities.Profile.Model;

namespace Frontend.Features.Services.Interfaces;

public interface IProfileService
{
    Task<UserProfileResponse> GetUserProfile(GetUserProfileRequest request);
}