using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace RouteFiltersExample
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
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services for MVC and routing.
            // In this example, we are using MVC to demonstrate route filters.
            services.AddRouting();
            services.AddControllers(options =>
            {
                // Registering a global action filter for all routes
                options.Filters.Add<CustomRouteFilter>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting(); // Middleware for routing HTTP requests to endpoints

            // Define the endpoint mappings
            app.UseEndpoints(endpoints =>
            {
                // Simple route that uses the custom route filter
                endpoints.MapGet("/products/{id}", async context =>
                {
                    var productId = context.Request.RouteValues["id"];
                    await context.Response.WriteAsync($"Product ID: {productId}");
                });

                // Another route using the same filter to demonstrate multiple uses
                endpoints.MapGet("/categories/{categoryId}", async context =>
                {
                    var categoryId = context.Request.RouteValues["categoryId"];
                    await context.Response.WriteAsync($"Category ID: {categoryId}");
                });
            });
        }
    }

    // Custom route filter implementation
    public class CustomRouteFilter : IRouteFilter
    {
        public Task OnRouteAsync(RouteContext context)
        {
            // Before the route handler executes, we can perform custom logic
            // For example, you can modify route values, inspect requests, or perform logging.

            var routeData = context.RouteData.Values;
            if (routeData.ContainsKey("id"))
            {
                var productId = routeData["id"];
                // Perform some custom logic before the route executes
                // In this case, we're logging the productId before the route is handled
                Console.WriteLine($"Product ID {productId} is being processed.");
            }

            // Continue to the next middleware or route handler
            return Task.CompletedTask;
        }
    }
}
