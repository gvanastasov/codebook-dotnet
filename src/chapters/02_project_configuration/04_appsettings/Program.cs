using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConfigApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // Introduction:
                    // Configuration management in ASP.NET Core allows you to manage settings and configurations for your application.
                    // The configuration system reads settings from various sources such as appsettings.json, environment variables, and command-line arguments.

                    // Understanding Configuration:
                    // The configuration system is built on a set of key-value pairs.
                    // Configuration settings can be organized into a hierarchical structure using sections.

                    // Benefits of Configuration Management:
                    // - Centralized management of application settings.
                    // - Support for environment-specific configurations.
                    // - Secure management of sensitive data using user secrets and environment variables.

                    // Add configuration from appsettings.json
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    // Add environment-specific configuration
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    // Add environment variables
                    config.AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Bind configuration settings to a strongly-typed object
            services.Configure<MySettings>(_configuration.GetSection(MySettings.SectionName));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    // Read configuration values directly from IConfiguration interface
                    // using the syntax: _configuration["SectionName:KeyName"]
                    var appName = _configuration[$"MySettings:ApplicationName"];
                    var appVersion = _configuration["MySettings:Version"];

                    // Read configuration values using strongly-typed object definition
                    // using the Options pattern, which can be injected as a service.
                    var mySettings = context.RequestServices.GetService<IOptions<MySettings>>()?.Value;
                    if (mySettings != null)
                    {
                        appName = mySettings.ApplicationName;
                        appVersion = mySettings.Version;
                    }

                    await context.Response.WriteAsync($"Application Name: {appName}\nVersion: {appVersion}");
                });
            });
        }
    }

    // Strongly-typed settings class
    public class MySettings
    {
        public const string SectionName = "MySettings";
        public string ApplicationName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }
}