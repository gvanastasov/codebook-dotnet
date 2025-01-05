using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AdvancedRoutingFeatures
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Introduction:
            // This chapter demonstrates some advanced routing features in ASP.NET Core:
            // 1. Handling unmatched routes with MapFallback.
            // 2. Using custom middleware after routing.
            
            // Enable routing middleware
            app.UseRouting();

            // Define endpoints with advanced routing features
            app.UseEndpoints(endpoints =>
            {
                // Basic route example
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to Advanced Routing!");
                });

                // MapFallback: Matches any route that does not match an existing route
                endpoints.MapFallback(async context =>
                {
                    await context.Response.WriteAsync("This is a fallback route for unmatched requests.");
                });
            });
        }
    }
}
