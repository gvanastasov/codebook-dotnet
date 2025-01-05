using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
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
            // Authorization determines what actions or resources a user is allowed to access.
            // Together with authentication, these mechanisms protect sensitive data and functionality in your applications.
            //
            // Use Cases:
            // - **Authorization**: Restrict access to specific roles or policies (e.g., Admin-only features).
            //
            // This chapter covers:
            // - Implementing a simple authorization policy.

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login"; // Redirect to this path for unauthenticated requests.
                });

            // Add authorization services with a simple policy.
            services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBeAdmin", policy =>
                    policy.RequireRole("Admin")); // Restrict access to users with the "Admin" role.
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseAuthentication();
            // Authorization verifies if the authenticated user has permissions to access resources.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to the public page! No authentication required.");
                });

                // Admin-only route: Requires "Admin" role.
                endpoints.MapGet("/admin", async context =>
                {
                    if (context.User.IsInRole("Admin"))
                    {
                        await context.Response.WriteAsync("Welcome, Admin!");
                    }
                    else
                    {
                        context.Response.StatusCode = 403; // Forbidden
                        await context.Response.WriteAsync("Access denied: Admins only.");
                    }
                }).RequireAuthorization("MustBeAdmin");

                // Login route: Simulates a login action.
                endpoints.MapGet("/login", async context =>
                {
                    // Simulate creating a new identity with "Admin" role.
                    var claims = new[] { new Claim(ClaimTypes.Name, "admin"), new Claim(ClaimTypes.Role, "Admin") };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    context.Response.Redirect("/admin");

                });

                // Logout route: Handles the logout process.
                endpoints.MapGet("/logout", async context =>
                {
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Response.Redirect("/");
                });
            });
        }
    }
}