using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace _02_mocking
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
            // Register dependencies and services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ProductController>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Mocking Dependencies Example");
                });
            });
        }
    }

    // Introduction:
    // Dependency mocking is a critical technique in unit testing that allows you to isolate the functionality
    // of a class or component by simulating its dependencies. This approach ensures that the tests are focused
    // only on the logic being tested, rather than external systems, services, or other dependencies.
    //
    // Mocking is particularly useful for:
    // - Simulating database calls without an actual database connection.
    // - Mimicking third-party API behavior during testing.
    // - Controlling the return values and behavior of dependencies to cover edge cases.
    //
    // This chapter demonstrates:
    // - How to use Moq, a popular library for creating mock objects in .NET.
    // - Testing a controller method that depends on a service.

    // Domain Model
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    // Service Interface
    public interface IProductService
    {
        List<Product> GetProducts();
    }

    // Service Implementation
    public class ProductService : IProductService
    {
        public List<Product> GetProducts()
        {
            // Normally, this would fetch data from a database or an external source
            return new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1200 },
                new Product { Id = 2, Name = "Phone", Price = 800 },
            };
        }
    }

    // Controller
    public class ProductController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public List<Product> Get()
        {
            return _productService.GetProducts();
        }
    }
}
