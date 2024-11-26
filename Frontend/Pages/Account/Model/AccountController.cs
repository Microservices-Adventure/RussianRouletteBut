using Frontend.Entities.Account.Model;
using Frontend.Features.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages.Account.Model;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
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
