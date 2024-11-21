using Frontend.Entities.Account.Model;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages.Account.Model;

public class AccountController : Controller
{
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
    public IActionResult Register(RegisterModel registerModel)
    {

        return RedirectToAction("RegisterSend", "Account");
    }

    public IActionResult RegisterSend(RegisterModel registerModel)
    {

        return View();
    }
}
