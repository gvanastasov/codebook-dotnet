using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace RouteParametersExample
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Introduction:
            // This chapter demonstrates how to define and use route parameters.
            // Route parameters allow you to capture dynamic values from the URL and use them in your application.

            // Enable routing middleware
            app.UseRouting();

            // Define endpoints that use route parameters
            app.UseEndpoints(endpoints =>
            {
                // Map a GET request to /user/{id:int}, where 'id' is a route parameter, and the expected value type
                // is specified as 'int', separated via a colon.
                endpoints.MapGet("/user/{id:int}", (int id) =>
                {
                    return $"User ID: {id}";
                });

                // Map a GET request to /product/{name}, where 'name' is a route parameter
                endpoints.MapGet("/product/{name}", (string name) =>
                {
                    return $"Product: {name}";
                });

                // Map a GET request with an optional route parameter
                // The '?' character makes the 'query' parameter optional.
                endpoints.MapGet("/search/{query?}", (string query) =>
                {
                    return string.IsNullOrEmpty(query) ? "Search query is empty." : $"Search results for: {query}";
                });

                // Map a GET request with a default value for the 'category' parameter
                // The 'category' parameter has a default value of 'all'.
                endpoints.MapGet("/category/{category=all}", (string category) =>
                {
                    return $"Category: {category}";
                });
            });
        }
    }
}
