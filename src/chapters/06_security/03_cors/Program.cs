using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CORSExample
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
            // CORS (Cross-Origin Resource Sharing) allows a web application hosted on one domain
            // to access resources hosted on another domain securely.
            //
            // Example:
            // - A front-end application running on "http://localhost:3000"
            //   needs to fetch data from an API running on "http://localhost:5000".
            //
            // Use Cases:
            // - Single Page Applications (SPAs) consuming APIs.
            // - Allowing trusted third-party applications to interact with your API.
            // - Securely enabling cross-domain resource sharing in controlled scenarios.
            //
            // This chapter demonstrates:
            // - How to configure CORS in ASP.NET Core.
            // - Using default, custom, and named CORS policies.
            //
            // Note:
            // to try out the cors errors from the client, the application is set to use
            // both http and https. You can run the client on http://localhost:5147 and see
            // no issue with any of the policies, 

            // Define a CORS policy named "AllowSpecificOrigins".
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:5147")
                        .AllowAnyHeader()                   
                        .AllowAnyMethod();                
                });

                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

                options.AddPolicy("DynamicOrigin", builder =>
                {
                    builder
                        .SetIsOriginAllowed(origin => origin.Contains("example.com"))
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }); 
            });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Explanation:
            // Enable CORS middleware. Ensure it is added before UseAuthorization.
            // You can specify the default (global) policy here.
            app.UseCors("AllowSpecificOrigins");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    // Example Controller: Demonstrates CORS in action.
    public class SampleController : ControllerBase
    {
        // Override and apply named policy to this endpoint
        [HttpGet("/public-data")]
        [EnableCors("AllowAll")]
        public IActionResult GetPublicData()
        {
            // Example: This endpoint can be accessed from allowed origins.
            return Ok(new { Message = "This is public data accessible via CORS!" });
        }

        [HttpGet("/restricted-data")]
        public IActionResult GetRestrictedData()
        {
            // Example: This endpoint requires additional CORS headers for secure access.
            return Ok(new { Message = "This is restricted data requiring CORS!" });
        }

        [HttpGet("/dynamic-origin")]
        [EnableCors("DynamicOrigin")]
        public IActionResult GetDynamicOrigin()
        {
            // Example: This endpoint allows dynamic origins based on the request.
            return Ok(new { Message = "This is a dynamic origin endpoint!" });
        }
    }

    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
