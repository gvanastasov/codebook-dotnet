using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttributeRoutingExample
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Introduction:
            // This chapter introduces attribute routing in ASP.NET Core.
            // Attribute routing allows you to define routes directly on the action methods of controllers.
            // It provides a more flexible way to configure routes, especially when you want to assign specific routes to actions.

            // Enable routing middleware
            app.UseRouting();

            // Define endpoints for controller actions with attribute routing
            app.UseEndpoints(endpoints =>
            {
                // Example controller with attribute routing
                endpoints.MapControllers();
            });
        }
    }

    // Controller with Attribute Routing
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // GET api/products
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Getting all products.");
        }

        // GET api/products/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok($"Getting product with ID: {id}");
        }

        // POST api/products
        [HttpPost]
        public IActionResult Post([FromBody] string product)
        {
            return CreatedAtAction(nameof(Get), new { id = 1 }, product);
        }

        // PUT api/products/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string product)
        {
            return Ok($"Updated product with ID: {id}, new name: {product}");
        }

        // DELETE api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok($"Deleted product with ID: {id}");
        }
    }
}
