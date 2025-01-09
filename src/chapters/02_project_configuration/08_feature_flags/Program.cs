using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FeatureTogglesExample
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
                    var env = hostingContext.HostingEnvironment;

                    // Introduction:
                    // Feature toggles (also known as feature flags) allow enabling or disabling features dynamically without deploying new code.
                    //
                    // Why Feature Toggles?
                    // - Gradual rollouts: Enable features for specific users or regions before full deployment.
                    // - A/B testing: Test variations of features to optimize user experience.
                    // - Quick rollbacks: Disable problematic features without downtime.
                    //
                    // Example:
                    // - A new UI feature is under development but not ready for all users. Use a toggle to expose it only to a beta group.
                    //
                    // This chapter demonstrates:
                    // - Using configuration to manage feature toggles.
                    // - Accessing feature flags in application logic.
                    // - A sample API that adapts behavior based on feature toggles.

                    // Add feature toggle configuration
                    config
                        .AddJsonFile("features.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
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
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    [ApiController]
    [Route("api/features")]
    public class FeatureController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FeatureController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // API Endpoint to check the status of a feature
        [HttpGet("{featureName}")]
        public IActionResult IsFeatureEnabled(string featureName)
        {
            var isEnabled = _configuration.GetValue<bool>($"Features:{featureName}");
            return Ok(new
            {
                Feature = featureName,
                IsEnabled = isEnabled
            });
        }

        // Example: Endpoint behavior depends on feature toggle
        [HttpGet("new-feature")]
        public IActionResult NewFeatureBehavior()
        {
            var isEnabled = _configuration.GetValue<bool>("Features:NewFeature");

            if (isEnabled)
            {
                return Ok("New feature is enabled!");
            }
            else
            {
                // Pretend the endpoint does not exist at all :)
                return NotFound();
            }
        }
    }
}
