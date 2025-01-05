using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace RoutePrioritizationExample
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
            // This chapter demonstrates how route order and prioritization work in ASP.NET Core.
            // Route order matters, and the first route that matches the request will be executed.
            // If multiple routes match, the first one registered will be used.

            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // Priority 1: More specific route comes first
                endpoints.MapGet("/products/{id}", async context =>
                {
                    var productId = context.Request.RouteValues["id"]?.ToString();
                    await context.Response.WriteAsync($"You requested a product with ID: {productId}");
                });

                // Priority 2: General route comes second, could match '/products/xyz'
                endpoints.MapGet("/products", async context =>
                {
                    await context.Response.WriteAsync("This is the list of products.");
                });

                // Priority 3: Catch-all route that will match any unmatched request
                endpoints.MapFallback(async context =>
                {
                    await context.Response.WriteAsync("This is a catch-all route for unmatched URLs.");
                });
            });
        }
    }
}
