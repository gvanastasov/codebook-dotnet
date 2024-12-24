using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace MiddlewareApp
{
    public class Startup
    {
        // Introduction:
        // Middleware is software that is assembled into an application pipeline to handle requests and responses.
        // Each component in the pipeline can either handle the request directly or pass it to the next middleware component.
        // Middleware can perform various tasks such as authentication, logging, error handling, and more.

        // Understanding Middleware:
        // Middleware components are executed in the order they are added to the pipeline.
        // The response flows back through the pipeline in reverse order.
        // Middleware can be built-in (provided by ASP.NET Core) or custom (user-defined).

        // Benefits of Middleware:
        // - Decoupling: Middleware components are independent and can be added or removed without affecting other components.
        // - Reusability: Middleware components can be reused across different applications.
        // - Maintainability: Middleware components are easier to maintain and test due to their modular nature.

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Built-in Middleware: Developer Exception Page
                // This middleware provides detailed error information when an exception occurs in the development environment.
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Built-in Middleware: Exception Handler and HSTS
                // Exception Handler: This middleware handles exceptions and displays a custom error page.
                // HSTS (HTTP Strict Transport Security): This middleware adds a Strict-Transport-Security header to responses.
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Built-in Middleware: HTTPS Redirection
            // This middleware redirects HTTP requests to HTTPS.
            app.UseHttpsRedirection();

            // Custom Middleware 1
            // This middleware writes a message before and after the next middleware in the pipeline.
            app.Use(async (context, next) =>
            {
                // Do work before the next middleware
                await context.Response.WriteAsync("Custom Middleware 1: Before\n");
                await next.Invoke();
                // Do work after the next middleware
                await context.Response.WriteAsync("Custom Middleware 1: After\n");
            });

            // Built-in Middleware: Static Files
            // This middleware serves static files such as HTML, CSS, JavaScript, and images.
            app.UseStaticFiles();

            // Custom Middleware 2
            // This middleware writes a message before and after the next middleware in the pipeline.
            app.Use(async (context, next) =>
            {
                // Do work before the next middleware
                await context.Response.WriteAsync("Custom Middleware 2: Before\n");
                await next.Invoke();
                // Do work after the next middleware
                await context.Response.WriteAsync("Custom Middleware 2: After\n");
            });

            // Built-in Middleware: Routing
            // This middleware routes requests to the appropriate endpoint.
            app.UseRouting();

            // Built-in Middleware: Authorization
            // This middleware authorizes a user to access secure resources.
            app.UseAuthorization();

            // Endpoint Middleware
            // This middleware maps endpoints to controllers.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}