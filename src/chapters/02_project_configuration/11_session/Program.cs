using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SessionAndCookiesExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Introduction:
            // Sessions and cookies are commonly used to maintain state in web applications.
            // - **Sessions**: Store data server-side with a unique identifier sent to the client via a cookie.
            // - **Cookies**: Store small amounts of data on the client side and are sent with every request to the server.
            //
            // Sessions are ideal for temporary data like user preferences during a single visit.
            // Cookies are useful for persistent data, such as "Remember Me" functionality.
            //
            // Security Considerations:
            // - Use HTTPS to encrypt cookies and session tokens.
            // - Use HttpOnly and Secure flags to protect cookies.
            // - Limit the size and scope of cookies to minimize attack surface.

            // Add session services
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true; // Prevent JavaScript access to the session cookie
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Require HTTPS
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Expire session after 30 minutes of inactivity
            });

            // Add controller and view services
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Enable session middleware
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/set-session")]
        public IActionResult SetSession(string key, string value)
        {
            HttpContext.Session.SetString(key, value); // Store data in session
            return Ok(new { Message = $"Session key '{key}' set to '{value}'" });
        }

        [HttpGet("/get-session")]
        public IActionResult GetSession(string key)
        {
            var value = HttpContext.Session.GetString(key); // Retrieve data from session
            if (value == null)
            {
                return NotFound(new { Error = $"Session key '{key}' not found." });
            }
            return Ok(new { Key = key, Value = value });
        }

        [HttpPost("/set-cookie")]
        public IActionResult SetCookie(string key, string value)
        {
            CookieOptions options = new CookieOptions
            {
                HttpOnly = true, // Prevent JavaScript access
                Secure = true,   // Require HTTPS
                Expires = DateTime.UtcNow.AddMinutes(30) // Set expiration time
            };
            Response.Cookies.Append(key, value, options); // Set cookie
            return Ok(new { Message = $"Cookie '{key}' set to '{value}'" });
        }

        [HttpGet("/get-cookie")]
        public IActionResult GetCookie(string key)
        {
            if (!Request.Cookies.TryGetValue(key, out var value))
            {
                return NotFound(new { Error = $"Cookie '{key}' not found." });
            }
            return Ok(new { Key = key, Value = value });
        }
    }
}
