using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace CachingConfigurationExample
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
            // Caching is a critical performance optimization technique that stores frequently accessed data
            // in memory or another fast-access layer to reduce database queries, computation, or API calls.
            //
            // Benefits of Caching:
            // - Improves application performance by reducing processing time.
            // - Reduces load on the database or other backend systems.
            // - Provides a better user experience with faster response times.
            //
            // Types of Caching in ASP.NET Core:
            // 1. In-Memory Caching (demonstrated here).
            // 2. Distributed Caching (e.g., Redis or SQL Server).
            // 3. Response Caching (for caching HTTP responses).
            //
            // This chapter demonstrates configuring and using in-memory caching with ASP.NET Core.

            // Register memory caching services
            services.AddMemoryCache();

            services.AddControllersWithViews();
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

    public class DataService
    {
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "ExpensiveData";

        public DataService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        // Method to get cached data or fetch it if not cached
        public async Task<string> GetExpensiveDataAsync()
        {
            // Check if data is already in the cache
            if (!_memoryCache.TryGetValue(CacheKey, out string data))
            {
                // Simulate expensive data fetching (e.g., database or API call)
                data = await Task.FromResult("Expensive data fetched at " + DateTime.Now);

                // Set cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                // Store data in cache
                _memoryCache.Set(CacheKey, data, cacheEntryOptions);
            }

            return data;
        }
    }

    // Controller to demonstrate caching
    public class HomeController : Controller
    {
        private readonly DataService _dataService;

        public HomeController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("/")]
        public async Task<string> Index()
        {
            // Fetch data using the caching service
            var data = await _dataService.GetExpensiveDataAsync();
            return $"Data: {data}";
        }
    }
}
