using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.HttpOverrides;

namespace HttpsEnforcement
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
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureKestrel(options => 
                        {
                            // Introduction:
                            // HTTPS ensures that all communication between the client and the server is encrypted
                            // and secure, protecting sensitive information such as login credentials and personal data.
                            // Encryption is achieved using SSL/TLS protocols, which encrypt data in transit. This is done
                            // via certificates that are issued by Certificate Authorities (CAs) to verify the identity of 
                            // the server.
                            // In local development environment, HTTPS is usually not enforced, but it can be demoed by
                            // using self-signed certificates. In other words, the server itself acts as the CA, issuing
                            // certificates to itself.

                            // Remarks:
                            // - you must trust the self-signed certificate in your browser to avoid security warnings.
                            //      - run the following command (windows dotnet cli): dotnet dev-certs https --trust
                            // - self-signed certificates are not suitable by any means for production use!

                            // Use Cases:
                            // - Redirect HTTP traffic to HTTPS to enforce secure communication.
                            // - Enable HSTS (HTTP Strict Transport Security) for production environments to instruct browsers
                            //   to only access the site over HTTPS.

                            // Load certificate for production use.
                            // In a production environment, you will use a trusted SSL certificate from a 
                            // Certificate Authority (CA), not a self-signed certificate. Here are the 
                            // general steps for setting up SSL in a production environment:

                            //  - Obtain an SSL certificate from a trusted provider (e.g., Let's Encrypt, DigiCert, etc.).
                            //  - Bind the certificate to your web server (IIS, Nginx, Apache, etc.).
                            //  - Configure your ASP.NET Core app to use the production certificate by setting up the Kestrel server or reverse proxy
                            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") 
                            {
                                options.Listen(IPAddress.Any, 443, listenOptions =>
                                {
                                    listenOptions.UseHttps("path_to_certificate.pfx", "your_password");
                                });
                            }
                        });
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // For production, you can configure redirection options as needed:
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                services.AddHttpsRedirection(options =>
                {
                    options.HttpsPort = 443; // Standard port for HTTPS
                });
            }

            // Optionally: enable middleware to forward headers.
            // This is useful when the app is hosted behind a load balancer or reverse proxy,
            // as it allows the app to read the original client IP address and protocol.
            // services.Configure<ForwardedHeadersOptions>(options =>
            // {
            //     options.ForwardedHeaders = 
            //         ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            // });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Explanation:
            // In development, enable the developer exception page for detailed error messages.
            // In production, use exception handling middleware instead.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // Enable HSTS (HTTP Strict Transport Security) in production.
                // This forces browsers to only use HTTPS for the domain for a specified time,
                // by sending a header, named "Strict-Transport-Security", to the client.
                app.UseHsts();
            }

            // Enable middleware to redirect HTTP requests to HTTPS.
            app.UseHttpsRedirection();

            app.UseRouting();

            // Map routes for controllers.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

    // Example Controller: Demonstrates the effect of HTTPS enforcement.
    public class HomeController : Controller
    {
        [HttpGet("/")]
        [RequireHttps] // Require HTTPS for this action.
        public IActionResult Index()
        {
            // Public endpoint accessible over HTTPS.
            return Content("Welcome to the secure page! You are accessing this over HTTPS.");
        }

        [HttpGet("/nonsecure")]
        public IActionResult NonSecure()
        {
            // This endpoint will redirect to HTTPS automatically due to middleware.
            return Content("If you accessed this over HTTP, you were redirected to HTTPS.");
        }
    }
}
