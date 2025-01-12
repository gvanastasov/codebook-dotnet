using _03_integration_testing; // Reference the main project
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace _03_integration_testing_test.Tests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                // Configure the test services (e.g., replace production DB with in-memory DB)
                builder.ConfigureServices(services =>
                {
                    // Replace the real DbContext with an in-memory version
                    services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase("IntegrationTestingDb"));
                });
            });

            // Create an HttpClient to send requests to the in-memory API
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_ReturnsEmptyList_WhenNoProductsExist()
        {
            // Act
            var response = await _client.GetAsync("/api/products");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<Product[]>(responseString);

            // Assert
            Assert.NotNull(products);
            Assert.NotEmpty(products);
        }

        [Fact]
        public async Task AddProduct_ThenGetProducts_ReturnsAddedProduct()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "Test Product",
                Price = 100m
            };

            var content = new StringContent(
                JsonSerializer.Serialize(newProduct),
                Encoding.UTF8,
                "application/json");

            // Act
            var addResponse = await _client.PostAsync("/api/products", content);
            addResponse.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync("/api/products");
            var responseString = await getResponse.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<Product[]>(responseString);

            // Assert
            Assert.Single(products);
            Assert.Equal("Test Product", products[0].Name);
            Assert.Equal(100m, products[0].Price);
        }
    }
}
