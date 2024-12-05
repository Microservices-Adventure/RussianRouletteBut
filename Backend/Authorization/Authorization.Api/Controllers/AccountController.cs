using Authorization.Domain.Models;
using Authorization.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IHostApplicationLifetime _lifetime;
    
    public AccountController(IAccountService accountService, IHostApplicationLifetime lifetime)
    {
        _accountService = accountService;
        _lifetime = lifetime;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserModel loginUserModel)
    {
        var result = await _accountService.Login(loginUserModel, _lifetime.ApplicationStopping);
        return Ok(result);
    }
}
