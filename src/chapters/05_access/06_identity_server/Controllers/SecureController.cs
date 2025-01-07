using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class SecureController : Controller
{
    public IActionResult Index()
    {
        var userName = User.Identity.Name;
        return View((object)$"Hello, {userName}! This is protected content.");
    }
}
