namespace MyShirtsApp.Test.Controllers
{
    using MyShirtsApp.Areas.Admin.Controllers;
    using MyShirtsApp.Services.Shirts;
    using MyShirtsApp.Test.Mocks;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;
    using MyShirtsApp.Data.Models;

    public class AdminShirtsControllerTest
    {
        [Fact]
        public void AllShouldReturnView()
        {
            var shirtService = new ShirtService(MapperMock.Instance, DatabaseMock.Instance);

            var shirtsController = new ShirtsController(shirtService);

            var result = shirtsController.All();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ChangeVisibiltyShouldReturnOk()
        {
            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUser",
                IsPublic = true
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.SaveChanges();

            var shirtService = new ShirtService(MapperMock.Instance, data);

            var shirtsController = new ShirtsController(shirtService);

            var result = shirtsController.ChangeVisibility(shirt.Id);

            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void ChangeVisibiltyShouldReturnBadRequest()
        {
            var shirtService = new ShirtService(MapperMock.Instance, DatabaseMock.Instance);

            var shirtsController = new ShirtsController(shirtService);

            var result = shirtsController.ChangeVisibility(1);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
