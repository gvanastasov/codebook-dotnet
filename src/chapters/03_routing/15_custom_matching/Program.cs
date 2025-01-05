using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CustomRouteMatchingExample
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
            // In this chapter, we will explore custom route matching in ASP.NET Core.
            // Custom route constraints allow you to implement your own logic to validate route parameters,
            // offering more control over routing behavior. This enables you to define complex rules
            // for matching URL parameters, such as ensuring a parameter matches a specific pattern or condition.

            // Register custom route constraints with the routing system.
            services.AddRouting(options =>
            {
                // Register the custom route constraint globally.
                // Here, we're mapping the name "custom" to the CustomRouteConstraint class.
                options.ConstraintMap.Add("custom", typeof(CustomRouteConstraint));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Custom route with a custom route constraint:
                // This route only matches if the "id" parameter starts with the letter "A".
                // The custom constraint is specified by the ":custom" suffix in the route template.
                endpoints.MapGet("/custom/{id:custom}", async context =>
                {
                    var id = context.Request.RouteValues["id"];
                    await context.Response.WriteAsync($"Custom Route Matched! ID: {id}");
                });
            });
        }
    }

    // Custom route constraint to match "id" starting with 'A'
    public class CustomRouteConstraint : IRouteConstraint
    {
        // Match logic for the custom route constraint.
        // The "Match" method is called to check if the "id" parameter matches the desired condition.
        public bool Match(HttpContext httpContext, IRouter router, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.ContainsKey("id"))
            {
                // Get the "id" parameter from the route values.
                var id = values["id"].ToString();
                
                // Check if the "id" starts with the letter 'A'.
                // This is the custom matching condition.
                return id.StartsWith("A");
            }

            // If the "id" parameter is not present or does not meet the condition, return false.
            return false;
        }
    }
}
