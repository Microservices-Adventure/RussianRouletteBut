using Frontend.Entities.Profile.Model;
using Frontend.Features.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages.Profile.Model;

public class ProfileController : Controller
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        UserProfileResponse result = await _profileService.GetUserProfile(
            new GetUserProfileRequest()
            {
                Username = HttpContext.User.Identity!.Name!
            });
        ViewData[nameof(UserProfileResponse)] = result;
        return View();
    }
}