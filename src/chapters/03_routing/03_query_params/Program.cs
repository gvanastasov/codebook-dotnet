using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace QueryStringParametersExample
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
            // This chapter demonstrates how to handle query string parameters in HTTP requests.
            // Query string parameters are passed in the URL after the '?' and are used to pass additional data.

            // Enable routing middleware
            app.UseRouting();

            // Define endpoints that handle query string parameters
            app.UseEndpoints(endpoints =>
            {
                // Map a GET request with a query parameter 'name'
                endpoints.MapGet("/greet", async context =>
                {
                    // Retrieve the 'name' query parameter
                    var name = context.Request.Query["name"];
                    var greeting = string.IsNullOrEmpty(name) ? "Hello, Guest!" : $"Hello, {name}!";
                    await context.Response.WriteAsync(greeting);
                });

                // Map a GET request with multiple query parameters: 'name' and 'age'
                endpoints.MapGet("/profile", async context =>
                {
                    // Retrieve the 'name' and 'age' query parameters
                    var name = context.Request.Query["name"];
                    var age = context.Request.Query["age"];
                    var responseMessage = string.IsNullOrEmpty(name)
                        ? "Name is required."
                        : string.IsNullOrEmpty(age)
                            ? $"Hello, {name}! Age is missing."
                            : $"Hello, {name}! You are {age} years old.";
                    await context.Response.WriteAsync(responseMessage);
                });
            });
        }
    }
}
