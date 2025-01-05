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
            // - **Policy-based Authorization**: Define more granular access rules using policies.
            //
            // Use Cases:
            // - **Policy-based**: Implement custom logic such as age-based restrictions or account verification.
            //
            // This chapter focuses on:
            // - Creating and applying policies for advanced scenarios.

            // Configure Authorization with policies.
            services.AddAuthorization(options =>
            {
                // Define a custom policy: Requires the "Admin" role and "IsVerified" claim.
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("VerifiedUser", policy =>
                    policy.RequireClaim("IsVerified", "true"));
            });

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

    // Example Controller: Demonstrates policy-based authorization.
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            // Public endpoint, no authorization required.
            return Content("Welcome to the public page!");
        }

        [HttpGet("/verified")]
        [Authorize(Policy = "VerifiedUser")] // Policy-based authorization.
        public IActionResult VerifiedUserPage()
        {
            return Content("Welcome to the Verified User page! Access requires the IsVerified claim.");
        }

        [HttpGet("/admin-verified")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminOnlyPage()
        {
            return Content("Welcome to the Admin Only page!");
        }
    }
}
