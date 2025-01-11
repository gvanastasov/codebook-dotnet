using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseConfigurationExample
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
            // Database configuration is a critical aspect of any application that involves persistent data storage.
            // A database can store, manage, and retrieve structured data efficiently. Proper configuration is essential
            // to ensure performance, security, and reliability. 
            //
            // Key Considerations for Database Configuration:
            // - Connection Strings: Define the location, authentication, and other parameters to connect to a database.
            // - Database Providers: Use the appropriate database provider (e.g., SQL Server, PostgreSQL, SQLite) for your application.
            // - Data Access Layer: Use tools like Entity Framework Core or Dapper to interact with the database.
            // - Migration and Seeding: Set up the schema and seed initial data to ensure the database is ready for use.
            //
            // This example demonstrates:
            // - Configuring an SQLite in-memory database for development and testing.
            // - Seeding the database with initial data.
            // - Providing endpoints to interact with the database.

            services.AddDbContext<AppDbContext>(options =>
            {
                // In-memory SQLite database is useful for testing and development
                options.UseSqlite("DataSource=file::memory:?cache=shared"); 

                // For a persistent SQLite database, use a file path:
                // options.UseSqlite("Data Source=app.db");
            });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext dbContext)
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

            // Ensure database is initialized, schema created, and seeded with data
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();
            SeedDatabase(dbContext);
        }

        private void SeedDatabase(AppDbContext dbContext)
        {
            // Description:
            // Seeds the database with initial data. This is useful for testing, demos, or prototyping.

            if (!dbContext.Products.Any())
            {
                dbContext.Products.AddRange(new List<Product>
                {
                    new Product { Name = "Sample Product 1", Price = 19.99m },
                    new Product { Name = "Sample Product 2", Price = 29.99m },
                    new Product { Name = "Sample Product 3", Price = 39.99m },
                });

                dbContext.SaveChanges();
            }
        }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ProductsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            return _dbContext.Products.ToList();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return Ok(product);
        }
    }
}
