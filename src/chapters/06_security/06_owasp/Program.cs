using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace OWASPSecurity
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
            // The OWASP (Open Web Application Security Project) Top 10 lists the most critical security 
            // vulnerabilities for web applications, including APIs. Securing APIs against these vulnerabilities 
            // is vital for protecting sensitive data, ensuring application integrity, and maintaining user trust.
            //
            // This chapter explores strategies to mitigate these vulnerabilities in an ASP.NET Core API (briefly):
            // - Injection Attacks
            // - Broken Authentication
            // - Sensitive Data Exposure
            // - XML External Entities (XXE)
            // - Broken Access Control
            // - Security Misconfiguration
            // - Cross-Site Scripting (XSS)
            // - Insecure Deserialization
            // - Using Components with Known Vulnerabilities
            // - Insufficient Logging & Monitoring

            // Enable controllers
            services.AddControllers();

            // Enable JWT Authentication for secure API access
            services
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://your-auth-provider.com";
                    options.Audience = "api";
                });

            // Add Logging for monitoring
            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enforce HTTPS
            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Log application startup
            logger.LogInformation("Securing API against OWASP Top 10 vulnerabilities");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    [ApiController]
    [Route("api")]
    public class SecurityDemoController : ControllerBase
    {
        private readonly ILogger<SecurityDemoController> _logger;

        public SecurityDemoController(ILogger<SecurityDemoController> logger)
        {
            _logger = logger;
        }

        // **Injection Attack Prevention**
        // Validate and sanitize inputs to avoid SQL or script injection.
        [HttpPost("validate-input")]
        public IActionResult ValidateInput([FromBody] string input)
        {
            if (Regex.IsMatch(input, @"[<>""';%&]"))
            {
                _logger.LogWarning("Potential injection detected in input: {Input}", input);
                return BadRequest("Invalid characters detected.");
            }

            return Ok(new { Message = "Input validated successfully", SanitizedInput = HtmlEncoder.Default.Encode(input) });
        }

        // **Broken Authentication Example**
        // Ensures that only authenticated users can access sensitive endpoints.
        [HttpGet("user-profile")]
        [Authorize]
        public IActionResult GetUserProfile()
        {
            var userId = User.FindFirst("sub")?.Value;

            if (userId == null)
            {
                _logger.LogWarning("Unauthorized access attempt");
                return Unauthorized("Authentication required.");
            }

            return Ok(new { UserId = userId, Name = "John Doe" });
        }

        // **Sensitive Data Exposure**
        // Demonstrates the secure handling of sensitive data.
        [HttpPost("process-sensitive-data")]
        public IActionResult ProcessSensitiveData([FromBody] string sensitiveData)
        {
            _logger.LogInformation("Sensitive data received: {Length} characters", sensitiveData.Length);

            // Do not log actual sensitive content; log its metadata only.
            return Ok(new { Message = "Sensitive data processed securely" });
        }

        // **XML External Entity (XXE) Prevention**
        // Disallow processing of external XML entities in XML parsers.
        [HttpPost("upload-xml")]
        public IActionResult UploadXml([FromBody] string xmlContent)
        {
            // Placeholder to demonstrate validation and parsing
            if (xmlContent.Contains("<!ENTITY"))
            {
                _logger.LogWarning("Potential XXE detected in XML input.");
                return BadRequest("Invalid XML content.");
            }

            return Ok(new { Message = "XML processed successfully" });
        }

        // **Broken Access Control**
        // Ensure that users can only access resources they are authorized for.
        [HttpGet("user/{userId}/data")]
        [Authorize]
        public IActionResult GetUserData(string userId)
        {
            var authenticatedUserId = User.FindFirst("sub")?.Value;

            if (authenticatedUserId != userId)
            {
                _logger.LogWarning("Access control violation: User {AuthenticatedUserId} attempted to access {UserId}", authenticatedUserId, userId);
                return Forbid();
            }

            return Ok(new { UserId = userId, Data = "Sensitive User Data" });
        }

        // **Cross-Site Scripting (XSS) Prevention**
        [HttpPost("process-html")]
        public IActionResult ProcessHtml([FromBody] string htmlContent)
        {
            var sanitizedHtml = HtmlEncoder.Default.Encode(htmlContent);
            return Ok(new { SanitizedHtml = sanitizedHtml });
        }

        // **Logging & Monitoring**
        // Log unusual patterns and potential attacks.
        [HttpGet("monitoring-example")]
        public IActionResult MonitoringExample()
        {
            _logger.LogWarning("Suspicious activity detected on endpoint");
            return Ok(new { Message = "Monitoring alert logged" });
        }
    }
}
