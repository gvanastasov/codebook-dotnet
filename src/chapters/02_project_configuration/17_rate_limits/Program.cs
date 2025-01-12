using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;

namespace RateLimitingExample
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
        /*
        -----------------------------------
        Rate Limiting: Control API Usage
        -----------------------------------
        Rate limiting is a technique used to control the number of requests that a user or service can make to an API or server within a specific period of time. 
        Its main purpose is to prevent abuse, ensure fair usage, and protect the server from overloads and attacks like Denial-of-Service (DoS).

        For example, you might want to limit the number of requests a user can make to an API endpoint, say, to 5 requests per minute.
        - If the user exceeds this limit, further requests are blocked or queued until the time window resets.

        Common Rate Limiting Strategies:
        - **Fixed Window**: This strategy allows a fixed number of requests within a specific time frame (e.g., 5 requests per minute). Once the time window resets, the limit is refreshed.
        - **Sliding Window**: The limit is applied over a sliding window of time, making it more dynamic. For example, it could limit requests in the last minute instead of a static window.
        - **Token Bucket**: This strategy allows requests to consume tokens, and once all tokens are used up, no more requests are allowed until tokens are replenished.

        In this chapter, we are using the **Fixed Window** strategy built into ASP.NET Core to limit requests to 5 per minute.
        */
        
        public void ConfigureServices(IServiceCollection services)
        {
            // Add rate limiting services to the application
            services.AddRateLimiter(options =>
            {
                // Define the Fixed Window Rate Limiter
                options.AddFixedWindowLimiter("FixedWindowLimiter", opts =>
                {
                    opts.PermitLimit = 5; // Max 5 requests per time window
                    opts.Window = TimeSpan.FromMinutes(1); // 1-minute time window
                    opts.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // Process the oldest requests first
                    opts.QueueLimit = 2; // Allow up to 2 requests in the queue
                });
            });

            // Add MVC services to allow controllers and views
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Enable rate limiting middleware
            // Note:
            // - order of middlewares matter!
            // - UseRateLimiter() should be called after UseRouting() and before UseEndpoints()
            app.UseRateLimiter();

            // Configure endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    /*
    -----------------------------------
    HomeController: Simple Controller to demonstrate rate limiting
    -----------------------------------
    - A basic API endpoint to show rate limiting in action.
    - Users can access the endpoint, but will be rate-limited if they exceed the allowed number of requests.
    */
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/api/test-rate-limit")]
        // We can enable rate limiting for specific actions using the EnableRateLimiting attribute
        [EnableRateLimiting("FixedWindowLimiter")]
        // or disable it via
        // [DisableRateLimiting]
        public IActionResult TestRateLimit()
        {
            return Ok("Request successful!");
        }
    }
}
