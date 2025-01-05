using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace ControllersExample
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
            // In this chapter, we introduce Controllers in ASP.NET Core.
            // Controllers are responsible for handling HTTP requests and returning responses.
            // Controllers are commonly used in MVC and Web API applications to separate the logic for handling requests.
            
            // Add controllers services to the DI container
            // Use AddControllers() for API-style applications (without views).
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // Map controller routes
                endpoints.MapControllers();
            });
        }
    }

    // Controller class
    // Controllers are typically marked with the [Controller] attribute or derived from ControllerBase (for APIs).
    // Each action within a controller responds to a specific route or HTTP verb (GET, POST, etc.).
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        // Action method to handle GET requests for /api/product
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            // Logic for fetching products (e.g., from a database)
            var products = new[] { "Product 1", "Product 2", "Product 3" };

            // Return the products as a JSON response
            return Ok(products);
        }

        // Action method to handle GET requests for /api/product/{id}
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = $"Product {id}";
            return Ok(product);
        }
    }
}
