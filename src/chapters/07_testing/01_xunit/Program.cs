using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _01_xunit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Introduction:
            // Unit testing is an essential part of modern software development. It ensures that individual components of an application
            // work as expected. xUnit is a popular testing framework for .NET applications, providing a simple and clean API for writing
            // tests.
            //
            // This chapter introduces you to unit testing with xUnit in ASP.NET Core. We'll demonstrate how to write tests, run them,
            // and validate application behavior.
            //
            // xUnit supports various types of tests, including unit tests, integration tests, and functional tests. For simplicity,
            // we will focus on unit testing, where we test individual methods or components in isolation.
            //
            // Benefits of Unit Testing:
            // - Detect bugs early: Unit tests catch bugs as soon as possible, helping to improve code quality.
            // - Ensure code reliability: Writing unit tests for functions makes the code more robust and ensures that the behavior is consistent.
            // - Confidence in refactoring: With a solid suite of tests, you can refactor the code with confidence that existing functionality is not broken.
            //
            // In this chapter, we will use xUnit to test a simple application component.
            // 
            // Note: check the test project - 01_xunit_test - and run the tests (via dotnet test) to see the results.

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IMyService, MyService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseRouting();

            // Map controllers to routes
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }

    // HomeController: Example controller to be tested
    public class HomeController : Controller
    {
        private readonly IMyService _myService;

        public HomeController(IMyService myService)
        {
            _myService = myService;
        }

        public IActionResult Index()
        {
            var result = _myService.GetGreeting();
            return View("Index", result);
        }
    }

    // IMyService: Interface for a service to be injected into the controller
    public interface IMyService
    {
        string GetGreeting();
    }

    // MyService: A simple service implementation
    public class MyService : IMyService
    {
        public string GetGreeting() => "Hello, world!";
    }
}
