using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace EfCoreExample
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
            /*
            ----------------------------------
            Object-Relational Mapping (ORM)
            ----------------------------------
            - ORM bridges the gap between the object-oriented world of programming languages and relational databases.
            - EF Core is Microsoft's ORM for .NET, enabling developers to interact with a database using .NET objects.
            - It supports LINQ queries, migrations, and schema management, making database access more intuitive.
            
            Key Benefits of EF Core:
            1. Simplifies CRUD operations by abstracting SQL queries.
            2. Provides database provider flexibility (SQLite, SQL Server, PostgreSQL, etc.).
            3. Supports migrations for evolving the database schema.
            */

            // Register EF Core with SQLite provider
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("DataSource=file::memory:?cache=shared"));

            // Add MVC services to the container
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
                endpoints.MapDefaultControllerRoute();
            });

            /*
            ----------------------------------
            Database Initialization and Seeding
            ----------------------------------
            - EnsureCreated() checks if the database exists and creates it if not.
            - This approach is ideal for prototyping but not recommended for production.
            - SeedDatabase() populates initial data for demonstration purposes.
            */

            dbContext.Database.EnsureCreated();
            SeedDatabase(dbContext);
        }

        private static void SeedDatabase(AppDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.AddRange(
                    new Product { Name = "Laptop", Price = 999.99M },
                    new Product { Name = "Smartphone", Price = 499.99M }
                );
                dbContext.SaveChanges();
            }
        }
    }

    /*
    ----------------------------------
    AppDbContext: The Database Context
    ----------------------------------
    - Serves as the bridge between the application and the database.
    - DbContext defines DbSet properties, which represent database tables.
    - EF Core uses the context to manage data access and change tracking.
    */
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet<Product> maps to the Products table in the database
        public DbSet<Product> Products { get; set; }
    }

    /*
    ----------------------------------
    Product: The Entity Model
    ----------------------------------
    - Represents the data structure of a database table.
    - EF Core uses this model to create the schema and map database records to objects.
    */
    public class Product
    {
        public int Id { get; set; } // Primary key
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    /*
    ----------------------------------
    HomeController: Fetching Data
    ----------------------------------
    - Interacts with the AppDbContext to retrieve data.
    - Demonstrates a basic GET action to display data from the database.
    */
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            var products = _dbContext.Products.ToList();
            return View(products);
        }
    }
}
