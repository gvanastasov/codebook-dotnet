using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundServicesExample
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
            // Background services are essential for tasks that need to run independently of user interaction.
            // These services operate continuously in the background, performing scheduled tasks, long-running operations,
            // or processing queues.
            //
            // Key Uses:
            // - Polling APIs or services for updates.
            // - Processing data in a queue (e.g., messaging systems like RabbitMQ).
            // - Running scheduled tasks (e.g., clearing temporary files).
            // - Performing asynchronous processing without blocking user requests.
            //
            // Example:
            // This chapter demonstrates a simple background service that writes timestamps to the console at regular intervals.
            //
            // Key Classes:
            // - `IHostedService`: Base interface for implementing background tasks.
            // - `BackgroundService`: A helper base class for easier implementation of long-running background tasks.

            services.AddHostedService<TimedHostedService>(); // Register the background service
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    // A background service that runs at regular intervals
    public class TimedHostedService : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(10); // Interval between executions

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Introduction:
            // The `ExecuteAsync` method runs in a loop until the service is stopped or the application shuts down.
            // Use this method to perform recurring tasks or long-running operations.

            while (!stoppingToken.IsCancellationRequested)
            {
                // Simulate background work
                Console.WriteLine($"[{DateTime.Now}] TimedHostedService is running.");
                await Task.Delay(_interval, stoppingToken); // Wait for the interval or cancellation
            }
        }
    }

    // Controller for basic interaction with the application
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public string Index()
        {
            return "Welcome to the Background Services Example! Check the console for background service logs.";
        }
    }
}
