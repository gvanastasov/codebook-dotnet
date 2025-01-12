using _02_mocking;
using Moq;

namespace _02_mocking_test;

public class UnitTest1
{
    // Unit Tests with Mocking
    public class ProductControllerTests
    {
        [Fact]
        public void Get_Returns_Products_From_Service()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();

            // Mock the GetProducts method to return a custom list
            var mockProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Mock Laptop", Price = 1000 },
                new Product { Id = 2, Name = "Mock Phone", Price = 700 },
            };

            mockProductService.Setup(service => service.GetProducts()).Returns(mockProducts);

            var controller = new ProductController(mockProductService.Object);

            // Act
            var result = controller.Get();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Mock Laptop", result.First().Name);
        }
    }
}
