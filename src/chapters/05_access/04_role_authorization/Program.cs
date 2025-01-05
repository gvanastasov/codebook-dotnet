using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthorizationBasics
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
            // Authorization is the process of determining what actions or resources a user is allowed to access.
            // ASP.NET Core provides two main approaches to authorization:
            // - **Role-based Authorization**: Restrict access based on user roles.
            //
            // Use Cases:
            // - **Role-based**: Grant or deny access to administrative areas based on user roles.
            //
            // This chapter focuses on:
            // - Configuring role-based authorization.

            // Configure Authorization with policies.
            services.AddAuthorization();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Explanation:
            // Enable authentication and authorization middleware to enforce rules.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Map a default controller route.
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

    // Example Controller: Demonstrates role-based and policy-based authorization.
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            // Public endpoint, no authorization required.
            return Content("Welcome to the public page!");
        }

        [HttpGet("/admin")]
        [Authorize(Roles = "Admin")] // Role-based authorization.
        public IActionResult Admin()
        {
            return Content("Welcome to the Admin page! Only users with the Admin role can access this.");
        }
    }
}
