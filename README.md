# codebook-dotnet
 
1. .NET Project Types
    - [Console Applications](./src/chapters/01_project_types/01_console_app/readme.md)
    - [Class Libraries](./src/chapters/01_project_types/02_class_library/dummy_app/Program.cs)
ASP.NET Core Web Applications
Worker Services
Windows Applications (WinForms, WPF)
Blazor Applications
.NET MAUI (Cross-platform)

Chapter 2: Dependency Injection (DI) in .NET
Understanding DI Basics
Built-in DI container in ASP.NET Core.
Using DI in Console and Worker projects.

Chapter 3: Middleware in ASP.NET Core
What is Middleware?
Built-in middleware vs custom middleware.
Order of middleware execution.

Chapter 4: RESTful APIs with ASP.NET Core
Building a simple API with controllers.
Exploring minimal APIs.
Adding Swagger for documentation.

Chapter 5: Blazor Basics
Blazor Server vs Blazor WebAssembly
Building a simple interactive web page.
Component structure.

Chapter 6: Entity Framework Core
Code-first database creation.
Querying and updating data.
Handling migrations.

Chapter 7: Cross-Platform Development with .NET MAUI
Setting up a cross-platform app.
Adding a simple UI.
Handling navigation.

Chapter 8: Unit Testing in .NET
Using xUnit for testing.
Mocking dependencies.
Writing integration tests.

Additional Chapters
Chapter 9: Advanced Configuration and Options
Centralized configuration in .NET using appsettings.json.
Binding configuration to strongly-typed objects.
Using environment variables and secrets for sensitive data.

Chapter 10: Logging in .NET
Built-in logging providers (Console, File, Debug).
Structured logging with Serilog or NLog.
Configuring logging levels and scopes.
Writing custom logging providers.

Chapter 11: Authentication and Authorization
Using ASP.NET Core Identity for user management.
Role-based and policy-based authorization.
Implementing OAuth2 and OpenID Connect with IdentityServer.
Using JWT (JSON Web Tokens) for stateless authentication.

Chapter 12: Real-Time Communication
Introduction to SignalR for real-time applications.
Setting up a basic SignalR hub.
Integrating SignalR with client-side Blazor or JavaScript.

Chapter 13: Globalization and Localization
Adding multi-language support in .NET applications.
Working with resource files.
Localizing dates, numbers, and currencies.

Chapter 14: Working with gRPC
Understanding gRPC and its use cases.
Creating a gRPC service in .NET.
Implementing client and server communication.

Chapter 15: Background Tasks and Scheduling
Using IHostedService for background tasks.
Setting up recurring tasks with Quartz.NET.
Handling long-running processes with Workers.

Chapter 16: Message Queues and Event-Driven Architectures
Integrating with RabbitMQ or Azure Service Bus.
Using .NET libraries for message queuing.
Implementing pub/sub models in microservices.

Chapter 17: Secure Coding Practices
Input validation and preventing SQL Injection.
Securing APIs against common vulnerabilities (OWASP Top 10).
Protecting sensitive data with encryption.

Chapter 18: Performance Optimization
Using tools like BenchmarkDotNet for profiling.
Optimizing memory usage and garbage collection.
Caching strategies for high-performance applications.

Chapter 19: Cloud Integration
Hosting .NET apps on Azure or AWS.
Using Azure Functions for serverless applications.
Integrating cloud storage (Blob Storage, S3) with .NET.

Chapter 20: Microservices and Distributed Systems
Introduction to microservices architecture.
Setting up API Gateway with Ocelot.
Managing communication between services (HTTP, gRPC, and messaging).
Service discovery and configuration with Consul or Kubernetes.

Chapter 21: Deployment and CI/CD
Packaging .NET applications for deployment.
Creating Docker images for .NET applications.
Setting up CI/CD pipelines with GitHub Actions or Azure DevOps.

Chapter 22: Debugging and Diagnostics
Using Visual Studio and Rider debugging tools.
Logging and tracing with Application Insights.
Diagnosing performance issues with dotnet-trace and PerfView.

Chapter 23: Exploring .NET Ecosystem Tools
Third-party libraries for .NET (e.g., Dapper, MediatR, Polly).
Extending the .NET CLI with custom tools.
Exploring Visual Studio extensions for productivity.

Chapter 24: Versioning and Backward Compatibility
Handling versioning in APIs.
Using compatibility packs in .NET.
Strategies for maintaining legacy applications.

Chapter 25: Practical Tips and Best Practices
Structuring .NET solutions for maintainability.
Best practices for exception handling and error reporting.
Writing clean and testable .NET code.