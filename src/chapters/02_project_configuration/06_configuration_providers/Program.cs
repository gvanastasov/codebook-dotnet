using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ConfigurationProvidersExample
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
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // Introduction:
                    // ASP.NET Core's configuration system retrieves key-value pairs using configuration providers.
                    // Each provider reads configuration values from a specific source (e.g., JSON files, environment variables, etc.).
                    //
                    // Why Configuration Providers?
                    // - Decouple application settings from code.
                    // - Allow seamless overrides in different environments (e.g., development, staging, production).
                    // - Support for hierarchical configuration structures (e.g., JSON objects).
                    //
                    // Examples of Configuration Providers:
                    // 1. JSON File Provider: Reads configuration from .json files like appsettings.json.
                    // 2. Environment Variable Provider: Retrieves configuration from environment variables.
                    // 3. Command-Line Provider: Reads configuration passed via command-line arguments.
                    // 4. In-Memory Provider: Stores configuration in memory for dynamic runtime updates.
                    //
                    // In this example:
                    // - `customsettings.json` is loaded if present.
                    // - Environment variables are loaded with the `AddEnvironmentVariables` method.
                    // - Command-line arguments are passed using `AddCommandLine`.
                    // - An in-memory collection demonstrates dynamic settings.

                    // Add custom configuration providers here
                    config
                        .AddJsonFile("customsettings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args)
                        .AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            { "InMemory:Setting1", "ValueFromMemory" },
                            { "InMemory:Setting2", "AnotherValueFromMemory" }
                        });
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
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
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

            // Logging example configurations
            logger.LogInformation("Configuration Values:");
            logger.LogInformation("CustomSetting1: {Value}", _configuration["CustomSetting1"]);
            logger.LogInformation("Environment: {Environment}", _configuration["Environment"]);
            logger.LogInformation("InMemory Setting1: {Value}", _configuration["InMemory:Setting1"]);
        }
    }

    [ApiController]
    [Route("api/config")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Endpoint to demonstrate fetching configuration values
        [HttpGet("get")]
        public IActionResult GetConfiguration([FromQuery] string key)
        {
            var value = _configuration[key];
            if (string.IsNullOrEmpty(value))
            {
                return NotFound(new { Error = $"Configuration key '{key}' not found." });
            }

            return Ok(new { Key = key, Value = value });
        }
    }
}
