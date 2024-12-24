using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleAppWithDI
{
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
            var host = CreateHostBuilder(args).Build();

            // Resolve the service and use it
            var myService = host.Services.GetRequiredService<IMyService>();
            myService.DoSomething();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Register the service with the DI container

                    // Transient: A new instance is created every time it is requested
                    services.AddTransient<IMyService, MyService>();

                    // Singleton: A single instance is created and shared across the application
                    // services.AddSingleton

                    // Scoped: A single instance is created for each scope
                    // services.AddScoped

                    // Register the service with a factory method
                    // services.AddTransient<IMyService>(serviceProvider => new MyService());

                    // Register the service with a factory method that depends on other services
                    // services.AddTransient<IMyService>(serviceProvider => new MyService(serviceProvider.GetRequiredService<IOtherService>()));
                });
    }
}