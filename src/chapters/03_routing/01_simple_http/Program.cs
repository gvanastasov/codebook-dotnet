using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleHttpRouting
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
            // HTTP routing in ASP.NET Core allows you to handle incoming requests
            // by mapping them to specific routes and handlers.
            // This chapter focuses on simple HTTP routing using MapGet, MapPost, MapPut, MapDelete, and MapFallback.
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            // Explanation:
            // UseEndpoints is used to define HTTP request routes.
            // Each route is mapped to a specific HTTP verb and a handler.
            app.UseEndpoints(endpoints =>
            {
                // MapGet: Handles GET requests
                endpoints.MapGet("/", async context =>
                {
                    // Returns a simple welcome message for the root URL.
                    await context.Response.WriteAsync("Welcome to Simple HTTP Routing!");
                });

                // MapPost: Handles POST requests
                endpoints.MapPost("/submit", async context =>
                {
                    // Simulates handling a POST request.
                    await context.Response.WriteAsync("POST request received at /submit");
                });

                // MapPut: Handles PUT requests
                endpoints.MapPut("/update", async context =>
                {
                    // Simulates handling a PUT request.
                    await context.Response.WriteAsync("PUT request received at /update");
                });

                // MapDelete: Handles DELETE requests
                endpoints.MapDelete("/delete", async context =>
                {
                    // Simulates handling a DELETE request.
                    await context.Response.WriteAsync("DELETE request received at /delete");
                });
            });
        }
    }
}
