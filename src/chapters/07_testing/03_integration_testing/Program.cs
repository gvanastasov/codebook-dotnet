using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace _03_integration_testing
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
            // Integration testing ensures that multiple components of an application work together as expected.
            // Unlike unit tests, which focus on individual pieces of code in isolation, integration tests validate
            // the interactions between different parts of the application (e.g., database, APIs, middleware).
            //
            // Example:
            // - Testing if an API correctly fetches data from the database and returns the expected response.
            // - Ensuring that middleware processes requests and responses as intended.
            //
            // Use Cases:
            // - Verifying the end-to-end behavior of an application.
            // - Identifying integration issues between different system components.
            // - Validating that database queries or API calls function correctly.
            //
            // This chapter demonstrates:
            // - Setting up an in-memory database for testing.
            // - Writing integration tests to validate API endpoints.

            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("IntegrationTestingDb"));
            services.AddControllers();
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
                endpoints.MapControllers();
            });
        }
    }

    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }

    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ProductsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<List<Product>> GetProducts()
        {
            return Ok(_dbContext.Products.ToList());
        }

        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
