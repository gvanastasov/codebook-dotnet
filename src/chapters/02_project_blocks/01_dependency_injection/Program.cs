using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleAppWithDI
{
    // Introduction:
    // Dependency Injection (DI) is a design pattern used to implement Inversion of Control (IoC) between classes and their dependencies.
    // It allows for better modularity, testability, and maintainability of code by decoupling the creation of dependencies from their usage.

    // Understanding DI:
    // DI involves three main roles: the service, the client, and the injector.
    // - Service: The class or component that provides certain functionality.
    // - Client: The class or component that depends on the service.
    // - Injector: The code or framework that injects the service into the client.

    // Benefits of DI:
    // - Decoupling: Reduces the tight coupling between classes and their dependencies.
    // - Testability: Makes it easier to unit test classes by allowing dependencies to be mocked or stubbed.
    // - Maintainability: Simplifies the management of dependencies and their lifecycles.

    // Interface for the service
    public interface IMyService
    {
        void DoSomething();
    }

    // Implementation of the service
    public class MyService : IMyService
    {
        public void DoSomething()
        {
            Console.WriteLine("MyService is doing something!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a HostBuilder to configure services and build the host
            var host = CreateHostBuilder(args).Build();

            // Resolve the service and use it
            var myService = host.Services.GetRequiredService<IMyService>();
            myService.DoSomething();
        }

        // Method to create and configure the HostBuilder
        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Register the service with the DI container

                    // Transient: A new instance is created every time it is requested
                    services.AddTransient<IMyService, MyService>();

                    // Singleton: A single instance is created and shared across the application
                    // services.AddSingleton<IMyService, MyService>();

                    // Scoped: A single instance is created for each scope (e.g., per HTTP request in a web application)
                    // services.AddScoped<IMyService, MyService>();

                    // Register the service with a factory method
                    // services.AddTransient<IMyService>(serviceProvider => new MyService());

                    // Register the service with a factory method that depends on other services
                    // services.AddTransient<IMyService>(serviceProvider => new MyService(serviceProvider.GetRequiredService<IOtherService>()));
                });
    }
}