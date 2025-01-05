using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace AuthBasics
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
            // Authentication is the process of verifying the identity of a user.
            // Authorization determines what actions or resources a user is allowed to access.
            // Together, these mechanisms protect sensitive data and functionality in your applications.
            //
            // Use Cases:
            // - **Authentication**: Ensure users log in before accessing private areas of an application.
            //
            // This chapter covers:
            // - Setting up cookie-based authentication.

            // Add authentication services with cookie-based authentication.
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login"; // Redirect to this path for unauthenticated requests.
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            // Explanation:
            // Enable authentication and authorization middleware.
            // Authentication checks the identity of a user.
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                // Public route: No authentication required.
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to the public page! No authentication required.");
                });

                // Protected route: Requires authentication.
                endpoints.MapGet("/protected", async context =>
                {
                    if (context.User.Identity?.IsAuthenticated ?? false)
                    {
                        await context.Response.WriteAsync($"Hello, {context.User.Identity.Name}! You are authenticated.");
                    }
                    else
                    {
                        context.Response.StatusCode = 401; // Unauthorized
                        await context.Response.WriteAsync("You must be logged in to access this page.");
                    }
                });

                // Login route: Simulates a login action.
                endpoints.MapGet("/login", async context =>
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, "JustMe") };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    context.Response.Redirect("/protected");
                });

                // Logout route: Simulates a logout action.
                endpoints.MapGet("/logout", async context =>
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Response.Redirect("/");
                });
            });
        }
    }
}
