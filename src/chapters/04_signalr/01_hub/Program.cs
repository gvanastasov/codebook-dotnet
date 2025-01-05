using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _01_hub
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
            // SignalR is a library for ASP.NET Core that simplifies adding real-time web functionality to applications.
            // Real-time functionality means that the server can push updates to connected clients as soon as they occur.
            //
            // Use Cases:
            // - **Chat Applications**: Build chat rooms or private messaging features where messages appear instantly.
            // - **Notifications**: Push alerts or updates to users in real time (e.g., stock prices, order updates).
            // - **Live Dashboards**: Display live data like analytics, IoT sensor readings, or system monitoring.
            // - **Collaborative Tools**: Enable features like collaborative editing or whiteboarding.
            // - **Gaming**: Implement multiplayer games with synchronized state across players.
            //
            // This chapter demonstrates how to set up a basic SignalR application, create a hub for communication,
            // and map it to an endpoint that clients can use to send and receive real-time updates.

            // Add SignalR services to the application.
            services.AddSignalR();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();

            // Explanation:
            // SignalR hubs are endpoints that handle client-server communication.
            // Here, the SignalR middleware is mapped to a `/chat` endpoint.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapRazorPages();
            });
        }
    }

    // Explanation:
    // A hub is the core component of SignalR that allows server-to-client and client-to-server communication.
    // This `ChatHub` handles a simple "SendMessage" operation, broadcasting messages to all connected clients.
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Clients.All ensures the message is sent to all connected clients.
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
