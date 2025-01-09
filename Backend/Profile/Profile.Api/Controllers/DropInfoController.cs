using Microsoft.AspNetCore.Mvc;
using Profile.Api.Models;
using Profile.Api.Services;

namespace Profile.Api.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IDropInfoService _dropInfoService;

        public ProfileController(IDropInfoService dropInfoService)
        {
            _dropInfoService = dropInfoService;
        }

        // POST: /api/profile/adduser
        [HttpPost("adduser")]
        public async Task<IActionResult> AddUserProfile([FromBody] AddUserProfileRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("UserProfile data is null.");
                }

                // Добавление пользователя через сервис
                var userProfile = await _dropInfoService.AddUserProfileAsync(request);

                // Возврат результата с кодом 201 (Created) и ссылкой на созданный ресурс
                return CreatedAtAction(nameof(GetUserProfile), new { username = userProfile.Username }, userProfile);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: /api/profile/getuser
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUserProfile([FromQuery] GetUserProfileRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Request data is null.");
                }

                // Получение пользователя через сервис
                var userProfile = await _dropInfoService.GetUserProfileAsync(request);
                UserProfileResponse userProfileResponse = new UserProfileResponse() { 
                    Username = userProfile.Username,
                    Id = userProfile.Id,
                    Email = userProfile.Email,
                    History = userProfile.History.ToList()
                };

                return Ok(userProfileResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: /api/profile/adddropinfo
        [HttpPost("adddropinfo")]
        public async Task<IActionResult> AddDropInfoByUsername([FromBody] AddDropInfoByUsernameRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("DropInfo data is null.");
                }

                // Добавление DropInfo через сервис
                var dropInfo = await _dropInfoService.AddDropInfoByUsernameAsync(request);

                // Возврат результата с кодом 201 (Created) и ссылкой на созданный ресурс
                return CreatedAtAction(nameof(GetUserProfile), new { username = request.Username }, dropInfo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}