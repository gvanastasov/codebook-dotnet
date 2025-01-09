using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace InputSanitizationExample
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
                endpoints.MapDefaultControllerRoute();
            });
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

    public class ProcessController : ControllerBase
    {
        [HttpPost("/process-input")]
        public IActionResult ProcessInput([FromForm] string userInput)
        {
            // Intentionally passing the raw input to simulate a security vulnerability
            // In a real-world scenario, you should sanitize the input before processing it
            // 
            // Herein: once you visit the default home page, a raw user input of <iframe src='javascript:parent.alert(`XSS`)'></iframe> is sent
            // which is then send back from the server to the client (raw) and then injected into the DOM. innerHTML is one way to inject
            // content into the DOM, and to a certain extent, it is safe. But with the above example - it is not :)
            var rawInput = userInput;

            // Escape input for safe HTML rendering
            var escapedInput = HtmlEncoder.Default.Encode(rawInput);

            // JSON encoding
            var jsonEncodedInput = JsonSerializer.Serialize(rawInput);

            // Trim, normalize spaces and remove potentially dangerous characters
            // Note: This is a basic example and should be customized based on the application's requirements.
            // For example, you may want to allow certain HTML tags or attributes.
            // You can also use a library like AntiXSS to sanitize input.
            var sanitizedInput = Regex.Replace(rawInput.Trim(), @"[<>""'/\s]+", " ");

            return Ok(new
            {
                RawInput = rawInput,
                SanitizedInput = sanitizedInput,
                EscapedInput = escapedInput,
                JsonEncodedInput = jsonEncodedInput
            });
        }
    }
}
