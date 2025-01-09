using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using System.Text.Json;
using FluentValidation.AspNetCore;

namespace FluentValidationExample
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
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Introduction:
            // FluentValidation is a popular .NET library used to define reusable validation rules
            // for models and ensure that data adheres to specific business rules before being processed.
            //
            // It allows you to write your validation logic in a fluent, readable way, and encapsulates
            // it in a reusable validator class, keeping the validation logic separate from the core
            // business logic of your application.
            //
            // Example:
            // - You have a user registration form where the user needs to provide their email address, 
            //   a password, and a name.
            // - FluentValidation allows you to create a validation rule for each field to ensure that
            //   they meet the correct format (e.g., email format, password length, etc.).
            //
            // Use Cases:
            // - Simplifying and reusing validation logic in complex models.
            // - Ensuring that incoming data adheres to certain rules before processing it further.
            // - Centralizing all validation rules in one place to enhance maintainability.
            //
            // This chapter demonstrates:
            // - How to use FluentValidation to define reusable validation rules for models.
            // - How to integrate FluentValidation into an ASP.NET Core application.
            // - Handling errors and feedback from the validation process.

            services.AddControllersWithViews();
            // Register FluentValidation services
            services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<Startup>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

    // Define a model that we want to validate
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // Create a FluentValidation validator for the User model
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            // Define validation rules for the User model
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }

    // Define a controller to handle user registration and validation
    public class HomeController : Controller
    {
        private readonly IValidator<User> _userValidator;

        public HomeController(IValidator<User> userValidator)
        {
            _userValidator = userValidator;
        }

        // The Home Index page where the registration form will be displayed
        [HttpGet("/")]
        public IActionResult Index()
        {
            var model = new User();
            return View(model);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromForm] User user)
        {
            // Validate the incoming user data using FluentValidation
            var validationResult = await _userValidator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                // If the validation fails, show the validation error messages on the same page
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View("Index", user);
            }
           
            TempData["SuccessMessage"] = "User successfully registered!";
            return RedirectToAction("Index");
        }
    }
}
