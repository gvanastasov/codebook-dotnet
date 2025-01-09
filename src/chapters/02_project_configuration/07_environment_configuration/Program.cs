using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EnvironmentConfigurationExample
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
                    // ASP.NET Core provides built-in support for environment-specific configuration management.
                    //
                    // Why Environment Configuration?
                    // - Separate configurations for development, staging, and production environments.
                    // - Ensures sensitive data (e.g., connection strings) is not exposed in code.
                    // - Simplifies testing and deployment pipelines.
                    //
                    // Key Features:
                    // - `IWebHostEnvironment`: Provides information about the hosting environment (e.g., Development, Staging, Production).
                    // - `appsettings.{EnvironmentName}.json`: Environment-specific JSON configuration files.
                    // - `Environment Variables`: Allows overrides for settings based on deployment needs.
                    //
                    // Example:
                    // - Development: Debug logs and a local database connection.
                    // - Production: Minimal logs and a secure database connection.
                    //
                    // This chapter demonstrates:
                    // - Using the environment to load specific configurations.
                    // - Accessing the current environment programmatically.
                    // - Logging environment-specific details for debugging purposes.
                    //
                    // Note:
                    /// - The environment is set using the `ASPNETCORE_ENVIRONMENT` environment variable.
                    /// - The default environment is `Production`.
                    /// - The environment can be set in Visual Studio, Docker, or the command line (dotnet run --environment Development).
                    /// - The environment can be accessed via `IWebHostEnvironment.EnvironmentName`.

                    var env = hostingContext.HostingEnvironment;

                    // Add environment-specific JSON configuration
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    // Load environment variables
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
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                logger.LogInformation("Environment: Development");
            }
            else if (_env.IsProduction())
            {
                logger.LogInformation("Environment: Production");
            }
            else if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Local")
            {
                logger.LogInformation("Environment: Local");
            }
            else
            {
                logger.LogInformation("Environment: {Environment}", _env.EnvironmentName);
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    [ApiController]
    [Route("api/environment")]
    public class EnvironmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EnvironmentController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // API Endpoint to fetch current environment details
        [HttpGet("current")]
        public IActionResult GetEnvironmentInfo()
        {
            return Ok(new
            {
                Environment = _env.EnvironmentName,
                Configuration = new
                {
                    DefaultSetting = _configuration["DefaultSetting"],
                    EnvSpecificSetting = _configuration["EnvSpecificSetting"],
                    MyCustomSetting = _configuration["MyCustomSetting"]
                }
            });
        }
    }
}
