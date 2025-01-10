# codebook-dotnet

## Chapters

### 1. .NET Project Types
  - [Console Applications](./src/chapters/01_project_types/01_console_app/readme.md)
  - [Class Libraries](./src/chapters/01_project_types/02_class_library/readme.md)
  - [ASP.NET Core Web Applications](./src/chapters/01_project_types/03_web_app/readme.md)
  - [Worker Services](./src/chapters/01_project_types/04_worker_service/readme.md)
  - [Windows Applications (WinForms, WPF)](./src/chapters/01_project_types/05_windows_app/readme.md)
  - [Blazor Applications](./src/chapters/01_project_types/06_blazor_app/readme.md)
  - [.NET MAUI (Cross-platform)](./src/chapters/01_project_types/07_maui/readme.md)

  - empty
  - [Azure Functions] todo
  - [Razor Class Libraries] todo
  - [Unit Test Projects] todo
  - [Xamarin Applications] todo
  - [API Projects (e.g., ASP.NET Core Web API)] todo

### 2. Project Configuration
  - [Dependency Injection (DI) in .NET](./src/chapters/02_project_blocks/01_dependency_injection/Program.cs)
  - [Middleware](./src/chapters/02_project_blocks/02_middleware/Program.cs)
  - [Logging](./src/chapters/02_project_configuration/03_logging/Program.cs)
  - [App Settings](./src/chapters/02_project_configuration/04_appsettings/Program.cs)
  - [User secrets](./src/chapters/02_project_configuration/05_user_secrets/Program.cs)
  - [Configuration Providers](./src/chapters/02_project_configuration/06_configuration_providers/Program.cs)
  - [Environment Configuration](./src/chapters/02_project_configuration/07_environment_configuration/Program.cs)
  - [Feature Toggles/Flags](./src/chapters/02_project_configuration/08_feature_flags/Program.cs)
  - [Localization](./src/chapters/02_project_configuration/09_localization/Program.cs)
  - [Health Checks](./src/chapters/02_project_configuration/10_health_check/Program.cs)
  - [Session and Cookies Configuration](./src/chapters/02_project_configuration/13_session_cookies/Program.cs)
  - [Database Configuration](./src/chapters/02_project_configuration/14_database_configuration/Program.cs)
  - [Background Services](./src/chapters/02_project_configuration/15_background_services/Program.cs)
  - [Custom Middleware Configuration](./src/chapters/02_project_configuration/16_custom_middleware/Program.cs)
  - [Application Insights and Monitoring](./src/chapters/02_project_configuration/17_application_insights/Program.cs)
  - [Application Secrets Management](./src/chapters/02_project_configuration/18_secrets_management/Program.cs)
  - [Caching Configuration](./src/chapters/02_project_configuration/19_caching/Program.cs)

- resources
- data
- ef

### 3. Routing
  - [Simple HTTP](./src/chapters/03_routing/01_simple_http/Program.cs)
  - [Route Parameters](./src/chapters/03_routing/02_route_params/Program.cs)
  - [Query Parameters](./src/chapters/03_routing/03_query_params/Program.cs)
  - [Fallback Route](./src/chapters/03_routing/04_fallback_route/Program.cs)
  - [Route Constraints](./src/chapters/03_routing/05_route_constraints/Program.cs)
  - [Route Wildcards](./src/chapters/03_routing/06_wildcards/Program.cs)
  - [Attribute Routing](./src/chapters/03_routing/07_attribute_routing/Program.cs)
  - [Razor Pages](./src/chapters/03_routing/08_razor_pages/Program.cs)
  - [Controllers](./src/chapters/03_routing/09_controllers/Program.cs)
  - [MVC](./src/chapters/03_routing/10_mvc/Program.cs)
  - [SPA](./src/chapters/03_routing/11_spa/Program.cs)
  - [Route Prioritization and Order](./src/chapters/03_routing/12_route_order/Program.cs)
  - [Route Matching and Performance](./src/chapters/03_routing/13_route_matching/Program.cs)
  - [Route Filters](./src/chapters/03_routing/14_route_filters/Program.cs)
  - [Custom Route Matching](./src/chapters/03_routing/15_custom_matching/Program.cs)
  - [Parameter Binding and Model Binding](./src/chapters/03_routing/16_model_binding/Program.cs)
  - [Route Data and Route Values](./src/chapters/03_routing/17_route_data/Program.cs)

- static files

### 4. Real-Time Communication & SignalR
  - [Setting up a basic SignalR hub](./src/chapters/04_signalr/01_hub/Program.cs)

### 5. Access
  - [Authentication Basics](./src/chapters/05_access/01_authentication/Program.cs)
  - [Authorization Basics](./src/chapters/05_access/02_authorization/Program.cs)
  - [ASP.NET Core Identity and User Management](./src/chapters/05_access/03_core_identity/Program.cs)
  - [Role-based authorization](./src/chapters/05_access/04_role_authorization/Program.cs)
  - [Policy-based authorization](./src/chapters/05_access/05_policy_authorization/Program.cs)
  - [Implementing OAuth2 and OpenID Connect with IdentityServer](./src/chapters/05_access/06_identity_server/Program.cs)
  - [Using JWT (JSON Web Tokens) for stateless authentication](./src/chapters/05_access/07_jwt/Program.cs)

### 6. Security
  - [HTTPS enforcement](./src/chapters/06_security/01_https/Program.cs)
  - [Cross-Site Request Forgery (CSRF) protection](./src/chapters/06_security/02_csrf_protection/Program.cs)
  - [Cross-Origin Resource Sharing (CORS)](./src/chapters/06_security/03_cors/Program.cs)
  - [Cross side scripting (XSS)](./src/chapters/06_security/04_sanitazation/Program.cs)
  - [Fluent Validation](./src/chapters/06_security/05_fluent_validation/Program.cs)
  - [Securing APIs against common vulnerabilities (OWASP Top 10)](./src/chapters/06_security/06_owasp/Program.cs)

### 6. Performance & Caching
- [Caching Strategies](./src/chapters/08_caching/01_caching/Program.cs)
  - [In-memory caching](./src/chapters/08_caching/02_in_memory_caching/Program.cs)
  - [Distributed caching (e.g., Redis)](./src/chapters/08_caching/03_distributed_caching/Program.cs)
  - [Response caching](./src/chapters/08_caching/04_response_caching/Program.cs)

- [Performance Optimization](./src/chapters/08_performance/01_performance_optimization/Program.cs)
  - [Response compression](./src/chapters/08_performance/02_response_compression/Program.cs)
  - [Minification of static files](./src/chapters/08_performance/03_minification/Program.cs)
  - [Profiling and diagnostics](./src/chapters/08_performance/04_profiling/Program.cs)

### 7. Background Tasks & Event-driven Architectures
- [Background Tasks and Scheduling](./src/chapters/09_background_tasks/01_background_tasks/Program.cs)
  - [Using IHostedService for background tasks](./src/chapters/09_background_tasks/02_ihostedservice/Program.cs)
  - [Setting up recurring tasks with Quartz.NET](./src/chapters/09_background_tasks/03_quartz_tasks/Program.cs)
  - [Handling long-running processes with Workers](./src/chapters/09_background_tasks/04_workers/Program.cs)

- [Message Queues and Event-Driven Systems](./src/chapters/10_message_queues/01_message_queues/Program.cs)
  - [Integrating with RabbitMQ or Azure Service Bus](./src/chapters/10_message_queues/02_rabbitmq_azure/Program.cs)
  - [Using .NET libraries for message queuing](./src/chapters/10_message_queues/03_message_queuing_libraries/Program.cs)
  - [Implementing pub/sub models in microservices](./src/chapters/10_message_queues/04_pub_sub/Program.cs)

### 8. Testing & Debugging
- [Unit Testing and Integration Testing](./src/chapters/11_testing/01_unit_testing/Program.cs)
  - [Using xUnit for testing](./src/chapters/11_testing/02_xunit/Program.cs)
  - [Mocking dependencies](./src/chapters/11_testing/03_mocking/Program.cs)
  - [Writing integration tests](./src/chapters/11_testing/04_integration_tests/Program.cs)

- [Debugging and Diagnostics](./src/chapters/12_debugging/01_debugging_tools/Program.cs)
  - [Using Visual Studio and Rider debugging tools](./src/chapters/12_debugging/02_vs_rider/Program.cs)
  - [Logging and tracing with Application Insights](./src/chapters/12_debugging/03_application_insights/Program.cs)
  - [Diagnosing performance issues with dotnet-trace and PerfView](./src/chapters/12_debugging/04_performance_diagnostics/Program.cs)

### 9. Cloud, Microservices, and Deployment
- [Cloud Integration](./src/chapters/13_cloud_integration/01_cloud/Program.cs)
  - [Hosting .NET apps on Azure or AWS](./src/chapters/13_cloud_integration/02_azure_aws/Program.cs)
  - [Using Azure Functions for serverless applications](./src/chapters/13_cloud_integration/03_azure_functions/Program.cs)
  - [Integrating cloud storage (Blob Storage, S3) with .NET](./src/chapters/13_cloud_integration/04_cloud_storage/Program.cs)

- [Microservices and Distributed Systems](./src/chapters/14_microservices/01_microservices_intro/Program.cs)
  - [Introduction to microservices architecture](./src/chapters/14_microservices/02_intro/Program.cs)
  - [Setting up API Gateway with Ocelot](./src/chapters/14_microservices/03_ocelot_gateway/Program.cs)
  - [Managing communication between services (HTTP, gRPC, and messaging)](./src/chapters/14_microservices/04_communication/Program.cs)
  - [Service discovery and configuration with Consul or Kubernetes](./src/chapters/14_microservices/05_service_discovery/Program.cs)

- [Deployment and CI/CD](./src/chapters/15_deployment/01_ci_cd/Program.cs)
  - [Creating Docker images for .NET applications](./src/chapters/15_deployment/02_docker/Program.cs)
  - [Setting up CI/CD pipelines with GitHub Actions or Azure DevOps](./src/chapters/15_deployment/03_cicd/Program.cs)