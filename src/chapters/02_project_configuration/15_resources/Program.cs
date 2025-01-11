using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace _15_resources
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
            // Resources in ASP.NET Core refer to static files (e.g., CSS, JS, images) and embedded files within the application.
            // Managing resources effectively ensures:
            // - Proper delivery of client-side assets.
            // - Ability to embed resources directly within the application assembly for portability.
            //
            // Types of Resources:
            // 1. Static Files: Files stored in the "wwwroot" directory or other configured locations.
            // 2. Embedded Resources: Files compiled into the application assembly.
            //
            // This chapter demonstrates:
            // - Configuring static files delivery.
            // - Using embedded resources in ASP.NET Core applications.

            // Add support for controllers with views
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Static Files Configuration:
            // Static files are served from the "wwwroot" folder by default.
            // Files are accessible directly via their relative path (e.g., "/css/site.css").
            app.UseStaticFiles();

            // Custom Static Files Configuration:
            // Additional static file locations can be added, as shown here:
            var staticFilePath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
            if (!Directory.Exists(staticFilePath))
            {
                Directory.CreateDirectory(staticFilePath);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilePath),
                RequestPath = "/Static"
            });

            // Embedded Resources Configuration:
            // Resources embedded in the assembly can be served through custom middleware or endpoints.
            // Note:
            // - Embedded resources are typically used for files that are not modified frequently.
            // - We must include them in the build (check the .csproj file).

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }
    }

    // Controller demonstrating embedded resource usage
    public class ResourceController : Controller
    {
        [HttpGet("/Resource/Embedded")]
        public IActionResult Embedded()
        {
            // Access the embedded resource
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "_15_resources.Resources.EmbeddedInfo.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string content = reader.ReadToEnd();
                return Content(content, "text/plain");
            }
        }
    }
}
