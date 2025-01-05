using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace RoutingToRazorPagesExample
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
            // Introduction:
            // This chapter demonstrates routing to Razor Pages.
            // Razor Pages allow you to handle web pages directly with URLs, simplifying the route management.

            // Register Razor Pages services in the DI container.
            // Razor Pages require the AddRazorPages() method to be called.
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // Map Razor Pages to the routing system.
                // When Razor Pages are mapped, they are automatically routed based on their location in the Pages folder.
                // For example, Pages/Index.cshtml will be available at /Index.
                endpoints.MapRazorPages();
            });
        }
    }
}
