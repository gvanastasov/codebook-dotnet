using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using _01_xunit;

namespace _01_xunit_test.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WithGreetingMessage()
        {
            // Arrange: Create a mock of IMyService
            var mockService = new Mock<IMyService>();
            mockService.Setup(service => service.GetGreeting()).Returns("Hello, world!");

            var controller = new HomeController(mockService.Object);

            // Act: Call the Index action
            var result = controller.Index();

            // Assert: Verify that the result is a ViewResult with the correct greeting message
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<string>(viewResult.Model);
            Assert.Equal("Hello, world!", model);
        }
    }
}
