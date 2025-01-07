using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly IIdentityServerInteractionService _interactionService;

    public AccountController(IIdentityServerInteractionService interactionService)
    {
        _interactionService = interactionService;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        // Render the login page with the returnUrl for redirection
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password, string returnUrl)
    {
        // Simulate user authentication (replace with actual logic)
        if (username == "testuser" && password == "password")
        {
            // Create user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("sub", username) // 'sub' is the subject claim in OpenID Connect
            };

            // Create the authentication cookie
            var identity = new ClaimsIdentity(claims, "password");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);

            // Redirect back to the return URL
            if (_interactionService.IsValidReturnUrl(returnUrl))
                return Redirect(returnUrl);

            return Redirect("~/");
        }

        // Show error if login fails
        ViewData["Error"] = "Invalid username or password.";
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("~/");
    }
}
