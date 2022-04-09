namespace MyShirtsApp.Test.Controllers
{
    using MyShirtsApp.Controllers;
    using MyShirtsApp.Services.Users;
    using MyShirtsApp.Test.Mocks;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Services.Users.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using Xunit;

    public class UsersControllerTest
    {
        [Fact]
        public void BecomeGetShouldWorksCorrectly()
        {
            var data = DatabaseMock.Instance;

            var userService = new UserService(data, null);

            var usersController = new UsersController(userService);

            var result = usersController.Become();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void BecomePostShouldReturnRedirectToAction()
        {
            var user = new User();

            var data = DatabaseMock.Instance;
            data.Users.Add(user);
            data.SaveChanges();

            var userService = new UserService(data, UserManagerMock.Instance);

            var usersController = new UsersController(userService);
            usersController.TempData = TempDataMock.Instance;
            usersController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var model = new BecomeSellerFormModel
            {
                CompanyName = "Test"
            };

            var result = usersController.Become(model);

            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void BecomePostShouldReturnViewWithModelState()
        {
            var user = new User();

            var data = DatabaseMock.Instance;
            data.Users.Add(user);
            data.SaveChanges();

            var userService = new UserService(data, UserManagerMock.Instance);

            var usersController = new UsersController(userService);
            usersController.TempData = TempDataMock.Instance;
            usersController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var model = new BecomeSellerFormModel();

            var result = usersController.Become(model);

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
