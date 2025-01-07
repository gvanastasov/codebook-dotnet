using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _06_identity_server
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
                    webBuilder.UseSetting("environment", "Development");
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Introduction:
            // IdentityServer4 is an OpenID Connect and OAuth 2.0 framework for ASP.NET Core.
            // It enables the implementation of centralized authentication and authorization for multiple applications.
            // This chapter demonstrates how to set up IdentityServer4 and configure a client application to use it.

            // Use Cases:
            // - Centralized Authentication: Manage user authentication in one place for multiple applications.
            // - API Security: Secure APIs by issuing and validating tokens.
            // - Single Sign-On (SSO): Allow users to log in once and access multiple applications.
            // - Federated Identity: Integrate with external identity providers like Google, Facebook, etc.

            // This chapter covers:
            // - Setting up IdentityServer4 with in-memory configuration.
            // - Configuring a client application to use IdentityServer4 for authentication.
            // - Demonstrating the authentication flow with OpenID Connect.

            // Note:
            // For demonstration purposes, the client application and IdentityServer are hosted in the same application.
            // This simplifies the setup and allows for easier testing and debugging.

            // Add IdentityServer
            services
                .AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(TestUsers.GetUsers());

            // Configure authentication
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "http://localhost:5064";
                    options.ClientId = "webapp_client";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.Scope.Add("profile");
                    options.SaveTokens = true;
                    options.RequireHttpsMetadata = false; // Disable HTTPS requirement for development
                    options.CallbackPath = "/signin-oidc"; // Ensure this matches the RedirectUris
                    options.SignedOutCallbackPath = "/signout-callback-oidc"; // Ensure this matches the PostLogoutRedirectUris
                });

            services.AddControllersWithViews();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            // Add IdentityServer middleware.
            app.UseIdentityServer();

            // Add authentication and authorization middleware.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}