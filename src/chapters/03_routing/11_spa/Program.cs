using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace ClientSideRoutingExample
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
            // This chapter demonstrates how to set up client-side routing with a catch-all route.
            // The backend should return the same `index.html` page for any route, allowing the client-side JavaScript to take over.
            
            // Add MVC services for views (we still need MVC to serve the initial page)
            // services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     // Enable Developer Exception Page for debugging purposes
            //     app.UseDeveloperExceptionPage();
            // }

            // Enable static file serving (e.g., for JavaScript, CSS, etc.)
            app.UseStaticFiles();

            // Enable routing middleware for processing HTTP requests
            app.UseRouting();

            // Map controllers to routes
            app.UseEndpoints(endpoints =>
            {
                // // Map the default controller route
                // endpoints.MapControllerRoute(
                //     name: "default",
                //     pattern: "{controller=Home}/{action=Index}/{id?}");

                // Catch-all route to handle client-side routing
                endpoints.MapFallbackToFile("index.html"); // Fallback to index.html for all non-API routes
            });
        }
    }

    // // MVC Controller to serve initial HTML page for client-side routing
    // public class HomeController : Controller
    // {
    //     public IActionResult Index()
    //     {
    //         // This action returns the initial HTML page that includes a script for client-side routing.
    //         return View();
    //     }
    // }
}
