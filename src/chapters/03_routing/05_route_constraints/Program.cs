using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace RouteConstraintsExample
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
            // Introduction:
            // This chapter demonstrates how to use route constraints in ASP.NET Core.
            // Route constraints allow you to restrict the acceptable values for route parameters.

            // Enable routing middleware
            app.UseRouting();

            // Define endpoints with route constraints
            app.UseEndpoints(endpoints =>
            {
                // Map a GET request with an 'id' parameter constrained to integers only
                endpoints.MapGet("/user/{id:int}", (int id) =>
                {
                    return $"User ID (int): {id}";
                });

                // Map a GET request with a 'date' parameter constrained to dates only
                endpoints.MapGet("/event/{date:datetime}", (DateTime date) =>
                {
                    return $"Event Date: {date.ToShortDateString()}";
                });

                // Map a GET request with a 'category' parameter constrained to specific values
                endpoints.MapGet("/category/{category:alpha}", (string category) =>
                {
                    return $"Category: {category}";
                });

                // Example of a more complex constraint (only allows values of 'admin' or 'user')
                endpoints.MapGet("/role/{role:regex(admin|user)}", (string role) =>
                {
                    return $"Role: {role}";
                });
            });
        }
    }
}
