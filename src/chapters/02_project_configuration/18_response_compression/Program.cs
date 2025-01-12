using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ResponseCompressionExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Introduction:
            // Response compression is the process of reducing the size of data transmitted between the server and the client.
            // By compressing HTTP responses, you can significantly reduce bandwidth usage and improve load times for your users.
            //
            // ASP.NET Core provides built-in middleware to enable response compression. The two most common compression algorithms
            // are Gzip and Brotli, both of which are supported by most modern browsers.
            //
            // Benefits of Response Compression:
            // - Reduced bandwidth usage: Compressing responses means less data needs to be transferred over the network.
            // - Faster load times: Smaller response sizes result in faster loading times for users, especially on mobile devices.
            // - Better performance: By reducing the size of the response, it can be processed and rendered more quickly.
            //
            // In this chapter, we'll show how to enable response compression in ASP.NET Core, and demonstrate the effects of
            // enabling Gzip and Brotli compression for different types of content.

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Enable response compression middleware
            builder.Services.AddResponseCompression(options =>
            {
                // Adding Gzip and Brotli compression providers
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Use response compression middleware
            app.UseResponseCompression();

            app.UseRouting();

            // Map controllers
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }

    // HomeController: Handles the Index action that returns compressed content.
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Simulate content (e.g., large JSON, text) to be compressed.
            var content = new string('A', 10000); // Simulating a large string

            // Return the simulated large content
            return View("Index", content);
        }
    }
}
