using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ParameterBindingAndModelBindingExample
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
            // In this chapter, we'll discuss parameter binding and model binding in ASP.NET Core.
            // Parameter binding allows you to map data from the HTTP request (query string, URL, body) 
            // to method parameters. Model binding extends parameter binding to complex data types, 
            // such as classes, and is typically used for POST requests to bind form data or JSON payloads.

            // Add MVC services to the container to support parameter and model binding.
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Setting up routing in the application to use controllers.
            app.UseRouting();

            // Setting up endpoints to handle incoming requests.
            app.UseEndpoints(endpoints =>
            {
                // Mapping a route that uses parameter binding for simple types.
                // The route expects a parameter "id" from the URL.
                endpoints.MapGet("/greet/{name}", async context =>
                {
                    // The "name" route parameter is automatically bound to the method parameter.
                    var name = context.Request.RouteValues["name"];
                    // Return a greeting message using the bound "name" parameter.
                    await context.Response.WriteAsync($"Hello, {name}!");
                });

                // Mapping a route that uses model binding to bind a complex object from a query string.
                endpoints.MapGet("/user", async context =>
                {
                    // Model binding to a User object from query string parameters (e.g., ?firstName=John&lastName=Doe).
                    var firstName = context.Request.Query["firstName"];
                    var lastName = context.Request.Query["lastName"];
                    var user = new User
                    {
                        FirstName = firstName,
                        LastName = lastName
                    };
                    // Returning a formatted message using the bound user model.
                    await context.Response.WriteAsync($"User: {user.FirstName} {user.LastName}");
                });

                // Example of using model binding with a POST request.
                // Here, we bind the incoming JSON body to the User model.
                endpoints.MapPost("/createuser", async context =>
                {
                    // Using the ASP.NET Core MVC binding system, we expect a JSON body to be automatically bound to a User object.
                    var user = await context.Request.ReadFromJsonAsync<User>();

                    // Returning a success message with the user's information.
                    await context.Response.WriteAsync($"User Created: {user.FirstName} {user.LastName}");
                });
            });
        }
    }

    // A simple User class to demonstrate model binding
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
