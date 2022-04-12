namespace MyShirtsApp.Test.Controllers
{
    using MyShirtsApp.Controllers;
    using MyShirtsApp.Services.Shirts;
    using MyShirtsApp.Test.Mocks;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void ErrorShouldReturnView()
        {
            var homeController = new HomeController(null);

            var result = homeController.Error();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void IndexShouldReturnView()
        {
            var shirtService = new ShirtService(MapperMock.Instance, DatabaseMock.Instance);

            var homeController = new HomeController(shirtService);

            var result = homeController.Index();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
