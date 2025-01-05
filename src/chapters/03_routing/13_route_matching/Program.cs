using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace RouteMatchingPerformanceExample
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
            // Add routing services which are necessary to map routes
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // Example 1: Simple route with a constraint
                // The route matches if the {id} is an integer (e.g., /products/123)
                // Using the {id:int} constraint improves performance by ensuring only integer values are matched
                endpoints.MapGet("/products/{id:int}", async context =>
                {
                    var productId = context.Request.RouteValues["id"]; // Extract the product ID from the route
                    await context.Response.WriteAsync($"Product ID: {productId}");
                });

                // Example 2: Route with multiple parameters and constraints
                // This route expects a {category} (string) and {id:int} (integer) for the product
                // The {id:int} constraint ensures that only valid integer IDs are matched, which improves route matching performance
                endpoints.MapGet("/products/{category}/{id:int}", async context =>
                {
                    var category = context.Request.RouteValues["category"]; // Extract category from the route
                    var productId = context.Request.RouteValues["id"]; // Extract product ID from the route
                    await context.Response.WriteAsync($"Category: {category}, Product ID: {productId}");
                });
            });
        }
    }
}
