using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MvcExample
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
            // This chapter demonstrates the use of MVC (Model-View-Controller) pattern in ASP.NET Core.
            // MVC separates concerns in an application by dividing it into three parts:
            // 1. Model: Represents data and business logic.
            // 2. View: Responsible for displaying the data to the user.
            // 3. Controller: Handles user input and updates the model and view accordingly.
            
            // Add MVC services to the DI container
            // Use AddControllersWithViews() to enable MVC support in an app (only includes controllers and views, but not api controllers).
            services.AddControllersWithViews();

            // Alternatively, you can use AddMvc() to add all MVC services (including views, api controllers, etc.)
            // services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            // Map controllers to routes and support views
            app.UseEndpoints(endpoints =>
            {
                // Map MVC controllers and views to routes
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    // MVC Controller
    // The controller in MVC handles incoming HTTP requests, interacts with the model, and returns a view (or result).
    public class HomeController : Controller
    {
        // Action method to handle GET requests for /Home/Index
        public IActionResult Index()
        {
            // Logic for fetching data (e.g., from a database)
            var welcomeMessage = "Welcome to MVC in ASP.NET Core!";

            // Return the view, passing the model data (welcomeMessage)
            return View(model: welcomeMessage);
        }

        // Action method to handle GET requests for /Home/About
        public IActionResult About()
        {
            var aboutMessage = "This is the About page of the MVC application.";
            return View(model: aboutMessage);
        }
    }
}
