namespace MyShirtsApp.Test.Controllers
{
    using System.Collections.Generic;
    using MyShirtsApp.Controllers;
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Services.Shirts;
    using MyShirtsApp.Services.Users;
    using MyShirtsApp.Test.Mocks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;
    using Microsoft.AspNetCore.Identity;
    using Moq;

    public class ShirtsControllerTest
    {
        [Fact]
        public void AllShouldReturnView()
        {
            var data = DatabaseMock.Instance;

            var mapper = MapperMock.Instance;

            var shirtService = new ShirtService(mapper, data);

            var userService = new UserService(data, null, SignInManagerMock.Instance);

            var shirtsController = new ShirtsController(shirtService, userService, mapper);
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var shirts = new List<ShirtServiceModel>();

            var model = new AllShirtsQueryModel
            {
                Shirts = shirts,
                CurrentPage = 0,
                ShirtsPerPage = 0,
                Size = 1,
                Sorting = ShirtSorting.PriceAsc,
                TotalShirts = 0,
            };

            var result = shirtsController.All(model);

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void DetailsShouldReturnView()
        {
            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUserId"
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.SaveChanges();

            var mapper = MapperMock.Instance;

            var shirtService = new ShirtService(mapper, data);

            var shirtsController = new ShirtsController(shirtService, null, mapper);

            var result = shirtsController.Details(shirt.Id, shirt.Name);

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void DetailsShouldReturnBadRequestWithInvalidShirt()
        {
            var data = DatabaseMock.Instance;

            var mapper = MapperMock.Instance;

            var shirtService = new ShirtService(mapper, data);

            var shirtsController = new ShirtsController(shirtService, null, mapper);

            var result = shirtsController.Details(1, null);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DetailsShouldBadRequestWithInvalidName()
        {
            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUserId"
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.SaveChanges();

            var mapper = MapperMock.Instance;

            var shirtService = new ShirtService(mapper, data);

            var shirtsController = new ShirtsController(shirtService, null, mapper);

            var result = shirtsController.Details(shirt.Id, "InvalidName");

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void AddGetShouldReturnView()
        {
            var data = DatabaseMock.Instance;

            var userService = new UserService(data, null, SignInManagerMock.Instance);

            var shirtsController = new ShirtsController(null, userService, null);
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var result = shirtsController.Add();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddPostShouldReturnRedirectToAction()
        {
            var user = new User
            {
                IsSeller = true
            };

            var data = DatabaseMock.Instance;

            data.Users.Add(user);
            data.SaveChanges();

            var shirtForm = new ShirtFormModel
            {
                Name = "Test",
                ImageUrl = "Test",
                Price = 30,
                SizeL = 1,
            };

            var shirtService = new ShirtService(MapperMock.Instance, data);

            var userService = new UserService(data, null, SignInManagerMock.Instance);

            var shirtsController = new ShirtsController(shirtService, userService, null);
            shirtsController.TempData = TempDataMock.Instance;
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var result = shirtsController.Add(shirtForm);

            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void AddPostShouldReturnViewWithModelState()
        {
            var user = new User
            {
                IsSeller = true
            };

            var data = DatabaseMock.Instance;

            data.Users.Add(user);
            data.SaveChanges();

            var shirtForm = new ShirtFormModel
            {
                Name = "Test",
                ImageUrl = "Test",
                Price = 30,
            };

            var shirtService = new ShirtService(MapperMock.Instance, data);

            var userService = new UserService(data, null, SignInManagerMock.Instance);

            var shirtsController = new ShirtsController(shirtService, userService, null);
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var result = shirtsController.Add(shirtForm);

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void MineShouldReturnView()
        {
            var user = new User();

            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = user.Id
            };

            var data = DatabaseMock.Instance;

            data.Users.Add(user);
            data.Shirts.Add(shirt);
            data.SaveChanges();

            var shirtService = new ShirtService(MapperMock.Instance, data);

            var shirtsController = new ShirtsController(shirtService, null, null);
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var result = shirtsController.Mine();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void EditGetShouldReturnView()
        {
            var user = new User();

            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = user.Id
            };

            var data = DatabaseMock.Instance;

            data.Users.Add(user);
            data.Shirts.Add(shirt);
            data.SaveChanges();

            var mapper = MapperMock.Instance;

            var shirtService = new ShirtService(mapper, data);

            var shirtsController = new ShirtsController(shirtService, null, mapper);
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var result = shirtsController.Edit(shirt.Id);

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void EditGetShouldReturnBadRequest()
        {
            var data = DatabaseMock.Instance;

            var mapper = MapperMock.Instance;

            var shirtService = new ShirtService(mapper, data);

            var shirtsController = new ShirtsController(shirtService, null, mapper);
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var result = shirtsController.Edit(1);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void EditGetShouldReturnUnauthorized()
        {
            var user = new User();

            var firstShirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = user.Id
            };

            var secondShirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "Test"
            };

            var data = DatabaseMock.Instance;

            data.Users.Add(user);
            data.Shirts.Add(firstShirt);
            data.Shirts.Add(secondShirt);
            data.SaveChanges();

            var mapper = MapperMock.Instance;

            var shirtService = new ShirtService(mapper, data);

            var shirtsController = new ShirtsController(shirtService, null, mapper);
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var result = shirtsController.Edit(secondShirt.Id);

            Assert.NotNull(result);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void EditPostShouldReturnRedirectToAction()
        {
            var user = new User();

            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = user.Id
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.Users.Add(user);
            data.SaveChanges();

            var mapper = MapperMock.Instance;

            var shirtService = new ShirtService(mapper, data);

            var shirtsController = new ShirtsController(shirtService, null, mapper);
            shirtsController.TempData = TempDataMock.Instance;
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var shirtForm = new ShirtFormModel
            {
                Name = "Test",
                ImageUrl = "Test",
                Price = 30,
                SizeL = 1,
            };

            var result = shirtsController.Edit(shirt.Id, shirtForm);

            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void EditPostShouldReturnBadRequest()
        {
            var data = DatabaseMock.Instance;

            var mapper = MapperMock.Instance;

            var shirtService = new ShirtService(mapper, data);

            var shirtsController = new ShirtsController(shirtService, null, mapper);

            var shirtForm = new ShirtFormModel
            {
                Name = "Test",
                ImageUrl = "Test",
                Price = 30,
                SizeL = 1,
            };

            var result = shirtsController.Edit(1, shirtForm);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void EditPostShouldReturnUnauthorized()
        {
            var user = new User();

            var firstShirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = user.Id
            };

            var secondShirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "Test"
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(firstShirt);
            data.Shirts.Add(secondShirt);
            data.Users.Add(user);
            data.SaveChanges();

            var mapper = MapperMock.Instance;

            var shirtService = new ShirtService(mapper, data);

            var shirtsController = new ShirtsController(shirtService, null, mapper);
            shirtsController.TempData = TempDataMock.Instance;
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var shirtForm = new ShirtFormModel
            {
                Name = "Test",
                ImageUrl = "Test",
                Price = 30,
                SizeL = 1,
            };

            var result = shirtsController.Edit(secondShirt.Id, shirtForm);

            Assert.NotNull(result);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void EditPostShouldReturnViewWithModelState()
        {
            var user = new User();

            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = user.Id
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.Users.Add(user);
            data.SaveChanges();

            var mapper = MapperMock.Instance;

            var shirtService = new ShirtService(mapper, data);

            var shirtsController = new ShirtsController(shirtService, null, mapper);
            shirtsController.TempData = TempDataMock.Instance;
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var shirtForm = new ShirtFormModel
            {
                Name = "Test",
                ImageUrl = "Test",
                Price = 30,
            };

            var result = shirtsController.Edit(shirt.Id, shirtForm);

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void DeleteShouldReturnOk()
        {
            var user = new User();

            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = user.Id
            };

            var data = DatabaseMock.Instance;

            data.Users.Add(user);
            data.Shirts.Add(shirt);
            data.SaveChanges();

            var shirtService = new ShirtService(null, data);

            var shirtsController = new ShirtsController(shirtService, null, null);
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var result = shirtsController.Delete(shirt.Id);

            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteShouldWorksReturnBadRequest()
        {
            var user = new User();

            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = user.Id
            };

            var data = DatabaseMock.Instance;

            data.Users.Add(user);
            data.Shirts.Add(shirt);
            data.SaveChanges();

            var shirtService = new ShirtService(null, data);

            var shirtsController = new ShirtsController(shirtService, null, null);
            shirtsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance("AnotherUser")
            };

            var result = shirtsController.Delete(shirt.Id);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
