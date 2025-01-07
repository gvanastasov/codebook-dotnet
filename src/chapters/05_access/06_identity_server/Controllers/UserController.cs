using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

public class UserController : Controller
{
    public IActionResult Login()
    {
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = "/secure"
        }, "oidc");
    }

    public IActionResult Logout()
    {
        return SignOut(new AuthenticationProperties
        {
            RedirectUri = "/"
        }, "Cookies", "oidc");
    }
}
