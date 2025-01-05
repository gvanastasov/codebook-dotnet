using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WildcardRoutesExample
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
            // This chapter demonstrates how to use wildcard routes in ASP.NET Core.
            // Wildcards allow you to match any part of the URL path, offering flexibility when routing.

            // Enable routing middleware
            app.UseRouting();

            // Define endpoints with wildcard routes
            app.UseEndpoints(endpoints =>
            {
                // Map a GET request to a route that can match any URL path under '/files'
                endpoints.MapGet("/files/*", async context =>
                {
                    // Capture the wildcard part and return it
                    var path = context.Request.Path.Value;
                    await context.Response.WriteAsync($"Wildcard route matched! Path: {path}");
                });

                // Map a GET request to catch any sub-path under '/docs'
                endpoints.MapGet("/docs/*", async context =>
                {
                    var docPath = context.Request.Path.Value;
                    await context.Response.WriteAsync($"Document path: {docPath}");
                });

                // Use wildcard for specific sub-path, such as '/images/{*imageName}'
                endpoints.MapGet("/images/{*imageName}", async context =>
                {
                    var imageName = context.Request.RouteValues["imageName"];
                    await context.Response.WriteAsync($"Image requested: {imageName}");
                });
            });
        }
    }
}
