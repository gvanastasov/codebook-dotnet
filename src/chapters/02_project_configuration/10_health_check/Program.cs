using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Linq;

namespace HealthChecksExample
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
            // Health Checks provide a way to monitor the health of an application and its dependencies.
            // They are used in production environments to detect and respond to issues such as:
            // - Database connectivity failures
            // - Insufficient disk space
            // - Degraded external service performance
            //
            // ASP.NET Core provides built-in support for health checks via the Health Checks API.
            // This example demonstrates how to:
            // 1. Add health checks for specific resources.
            // 2. Create a custom health checks response in JSON format.
            // 3. Display health check results in a user-friendly manner.

            services.AddHealthChecks()
                .AddCheck("DatabaseConnection", () =>
                {
                    // Simulate a database connection health check
                    bool isDatabaseConnected = true; // Replace with actual database connection logic
                    return isDatabaseConnected
                        ? HealthCheckResult.Healthy("Database is connected.")
                        : HealthCheckResult.Unhealthy("Database connection failed.");
                })
                .AddCheck("DiskSpace", () =>
                {
                    // Simulate a disk space health check
                    bool isDiskSpaceSufficient = true; // Replace with actual disk space logic
                    return isDiskSpaceSufficient
                        ? HealthCheckResult.Healthy("Sufficient disk space.")
                        : HealthCheckResult.Degraded("Low disk space.");
                });
            
            // Enable controllers and views for the web application
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();

                // Health Check Endpoint
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = async (context, report) =>
                    {
                        context.Response.ContentType = "application/json";

                        // Custom JSON format for health check response
                        var response = new
                        {
                            status = report.Status.ToString(),
                            checks = report.Entries.Select(entry => new
                            {
                                name = entry.Key,
                                status = entry.Value.Status.ToString(),
                                description = entry.Value.Description
                            }),
                            totalDuration = report.TotalDuration.TotalMilliseconds + " ms"
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
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
    }
}
