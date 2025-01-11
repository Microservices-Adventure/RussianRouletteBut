using System.Security.Claims;
using Frontend.Entities.Account.Model;
using Frontend.Features.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages.Account.Model;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;   
    private readonly ILogger<AccountController> _logger;
    
    public AccountController(IAccountService accountService, ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }
    
    public IActionResult Login()
    {
        return View();
    }
    
    public async Task<IActionResult> Logout()
    {
        HttpContext.Response.Cookies.Delete("accessToken");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }
    
    [Authorize]
    public string Closed()
    {
        return "Closed";
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var loginResponse = await _accountService.Login(loginModel);
        
        HttpContext.Response.Cookies.Append("accessToken", loginResponse.Token);
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, loginResponse.Username), new Claim(ClaimTypes.Email, loginResponse.Email) };
        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel registerModel)
    {
        await _accountService.Register(registerModel);
        return RedirectToAction("RegisterSend", "Account", registerModel);
    }

    public IActionResult RegisterSend(RegisterModel registerModel)
    {
        ViewData["username"] = registerModel.Username;
        return View();
    }
}
