// ================================================================================
// Chapter: ASP.NET Core Fundamentals
// ================================================================================
//
// Description:
// This chapter covers the core building blocks of an ASP.NET Core application, 
// explaining the Host, Services, Configuration, Middleware, and how they work 
// together to create the foundation of an application.
//
// Key Concepts Covered:
// - Host: The backbone that manages the application's lifecycle.
// - Services: Dependency injection and service management.
// - Configuration: Centralized application configuration system.
// - Middleware: Request pipeline components for handling HTTP requests.
// - The Program.cs and Startup.cs files, where the app is configured.
//
// ================================================================================

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fundamentals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // The Host is the core component that sets up and runs the application.
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Configures the Host, which is the backbone of an ASP.NET Core app.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <returns>An IHostBuilder instance configured with default settings.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args) // Preconfigured host with logging, DI, and configuration.
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); // Configures app-specific behavior.
                });
    }

    /// <summary>
    /// Startup class defines services and middleware for the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Registers application services with the DI container. (more on each later)
        /// </summary>
        /// <param name="services">Service collection for adding services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Services provide reusable functionality like MVC, Razor Pages, etc.
            services.AddControllersWithViews(); // Adds MVC services for controllers and views.
            services.AddRazorPages();           // Adds Razor Pages services.
        }

        /// <summary>
        /// Configures middleware for request processing. (more on each later)
        /// </summary>
        /// <param name="app">Application builder to define the middleware pipeline.</param>
        /// <param name="env">Hosting environment for environment-specific configuration.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Environment-specific middleware configuration.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Detailed error page for developers.
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Generic error handling.
                app.UseHsts();                          // Enforces HTTPS in production.
            }

            app.UseHttpsRedirection(); // Redirects HTTP to HTTPS.
            app.UseStaticFiles();      // Serves static files like CSS, JS, and images.

            app.UseRouting(); // Enables endpoint-based routing.

            app.UseAuthorization(); // Adds support for authorization checks.

            // Configures routes (endpoints).
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"); // Default route.
                endpoints.MapRazorPages(); // Maps Razor Pages endpoints.
            });
        }
    }
}
