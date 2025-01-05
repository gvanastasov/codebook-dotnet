using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace RouteDataAndRouteValuesExample
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
            // In this chapter, we will explore route data and route values in ASP.NET Core.
            // Route data represents the parameters matched by the routing system when a request is processed.
            // Route values are part of the route data and can be used to pass dynamic values from the route
            // to the handler method. We will show how to access and use these values for custom logic in handling requests.
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Set up routing to handle incoming HTTP requests.
            app.UseRouting();

            // Define endpoints for different routes.
            app.UseEndpoints(endpoints =>
            {
                // Mapping a simple route with route parameters.
                // This route accepts two parameters: "category" and "productId".
                endpoints.MapGet("/products/{category}/{productId}", async context =>
                {
                    // Route values are extracted from the context's RouteValues collection.
                    var category = context.Request.RouteValues["category"];
                    var productId = context.Request.RouteValues["productId"];

                    // Return a message using the route values extracted from the URL.
                    await context.Response.WriteAsync($"Category: {category}, Product ID: {productId}");
                });

                // Another example of a route with a query parameter.
                // We will extract the "id" route parameter and the "filter" query parameter.
                endpoints.MapGet("/search/{id}", async context =>
                {
                    // Accessing route values and query string values.
                    var id = context.Request.RouteValues["id"];
                    var filter = context.Request.Query["filter"];

                    // Display the extracted values as part of the response.
                    await context.Response.WriteAsync($"Search ID: {id}, Filter: {filter}");
                });

                // A route that uses a fallback to capture unmatched URLs.
                // This is an example of a fallback route that doesn't have defined route parameters.
                endpoints.MapFallback(async context =>
                {
                    // Accessing all route values for fallback.
                    var routeValues = context.GetRouteData()?.Values;
                    var routeData = routeValues != null ? string.Join(", ", routeValues) : "No route values matched.";
                    // Returning the captured route values for debugging purposes.
                    await context.Response.WriteAsync($"Fallback: Route values - {routeData}");
                });
            });
        }
    }
}
