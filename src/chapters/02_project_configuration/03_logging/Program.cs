using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoggingApp
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
                .ConfigureLogging(logging =>
                {
                    // Introduction:
                    // Logging is an essential part of any application, providing insights into the application's behavior and helping with debugging and monitoring.
                    // ASP.NET Core provides a built-in logging framework that supports various logging providers, such as console, debug, and third-party providers like Serilog and NLog.

                    // Understanding Logging:
                    // Logging involves capturing and storing information about the application's execution.
                    // Logs can have different levels of severity, such as Information, Warning, Error, and Critical.
                    // Logging providers are responsible for storing and displaying log messages.

                    // Benefits of Logging:
                    // - Debugging: Helps identify and diagnose issues in the application.
                    // - Monitoring: Provides insights into the application's performance and behavior.
                    // - Auditing: Keeps a record of important events and actions for security and compliance purposes.

                    // Configure logging to use Console and Debug providers.
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Custom Middleware for Logging
            // Logs the request path and response status code.
            app.Use(async (context, next) =>
            {
                // Log the request path
                logger.LogInformation("Handling request: {Path}", context.Request.Path);
                await next.Invoke();
                // Log the response status code
                logger.LogInformation("Finished handling request. Response status code: {StatusCode}", context.Response.StatusCode);
            });

            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/", () => "Hello World!");
            });
        }
    }
}