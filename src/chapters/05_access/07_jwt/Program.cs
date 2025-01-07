using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace _07_jwt_authentication
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
        // Store the secret key globally (not recommended for production)
        public static string SecretKey { get; private set; }

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Introduction:
            // In this chapter, we demonstrate how to use JWT (JSON Web Tokens) for stateless authentication.
            // JWT is a compact, URL-safe means of representing claims to be transferred between two parties.
            // The authentication is stateless, meaning no session data is stored on the server; the token itself holds the user information.

            // Use Cases:
            // - Stateless Authentication: The server does not store session information.
            // - API Authentication: Secure APIs using JWT tokens.
            // - Single Sign-On (SSO): Allow users to authenticate across multiple applications via JWT.

            // This chapter covers:
            // - JWT Token Generation: How to generate and issue a JWT token for authenticated users.
            // - JWT Token Validation: How to validate the JWT token to authenticate users in your API.

            // Once the program is running (dotnet run), you can test the API using Postman, curl or similar.
            // 1. Send a POST request to /api/auth/login with the following JSON payload:
            // {
            //     "Username": "testuser",
            //     "Password": "password"
            // }
            // 2. If the credentials are correct, the API will return a JWT token.

            // Generate a random 256-bit secret key at runtime (this is entirely for demo purpose. Consider
            // using a secure key management system in production).
            SecretKey = GenerateSecureKey();

            // Configure JWT authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
                    };
                });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Use Authentication Middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private string GenerateSecureKey()
        {
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[32];
                randomNumberGenerator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }

    // Controller for issuing and validating the JWT token
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;

            // Use the same SecretKey generated at startup (not recommended for production)
            _secretKey = Startup.SecretKey;
        }

        // Endpoint to generate JWT token
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            if (model.Username == "testuser" && model.Password == "password")
            {
                // Create a JWT token
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }
    }

    // Login request model
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
