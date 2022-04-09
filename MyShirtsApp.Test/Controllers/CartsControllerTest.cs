namespace MyShirtsApp.Test.Controllers
{
    using MyShirtsApp.Controllers;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Services.Carts;
    using MyShirtsApp.Test.Mocks;
    using MyShirtsApp.Models.Carts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class CartsControllerTest
    {
        [Fact]
        public void AddShouldReturnOk()
        {
            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUser",
                IsPublic = true
            };

            var size = new Size
            {
                Name = "XS"
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.Sizes.Add(size);
            data.SaveChanges();

            var cartsService = new CartService(MapperMock.Instance, data);

            var cartsController = new CartsController(cartsService);
            cartsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var result = cartsController.Add(shirt.Id, size.Name);

            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void AddShouldReturnBadRequest()
        {
            var data = DatabaseMock.Instance;

            var cartsService = new CartService(MapperMock.Instance, data);

            var cartsController = new CartsController(cartsService);
            cartsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var result = cartsController.Add(1, "Test");

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void MineShouldReturnViewWithCartAndSum()
        {
            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUser",
                IsPublic = true
            };

            var size = new Size { Name = "XS" };

            var cart = new Cart { UserId = "TestUser" };

            var shirtCart = new ShirtCart
            {
                Cart = cart,
                Shirt = shirt,
                SizeName = size.Name
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.Sizes.Add(size);
            data.Carts.Add(cart);
            data.ShirtCarts.Add(shirtCart);
            data.SaveChanges();

            var cartsService = new CartService(MapperMock.Instance, data);

            var cartsController = new CartsController(cartsService);
            cartsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var result = cartsController.Mine();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void DeleteShouldReturnOk()
        {
            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUser",
                IsPublic = true
            };

            var size = new Size
            {
                Name = "XS"
            };

            var cart = new Cart
            {
                UserId = "TestUser"
            };

            var shirtCart = new ShirtCart
            {
                Cart = cart,
                Shirt = shirt,
                SizeName = size.Name,
                Count = 2
            };

            var shirtSize = new ShirtSize
            {
                Shirt = shirt,
                Size = size,
                Count = 1
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.Sizes.Add(size);
            data.Carts.Add(cart);
            data.ShirtCarts.Add(shirtCart);
            data.ShirtSizes.Add(shirtSize);
            data.SaveChanges();

            var cartsService = new CartService(MapperMock.Instance, data);

            var cartsController = new CartsController(cartsService);
            cartsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var query = new CartQueryViewModel
            {
                Flag = false,
                ShirtId = shirt.Id,
                SizeName = size.Name
            };

            var result = cartsController.DeleteShirt(query);

            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeleteShouldReturnBadRequest()
        {
            var data = DatabaseMock.Instance;

            var cartsService = new CartService(MapperMock.Instance, data);

            var cartsController = new CartsController(cartsService);
            cartsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var query = new CartQueryViewModel
            {
                Flag = false,
                ShirtId = 1,
                SizeName = "test"
            };

            var result = cartsController.DeleteShirt(query);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void ClearShouldReturnOk()
        {
            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUser",
                IsPublic = true
            };

            var size = new Size
            {
                Name = "XS"
            };

            var cart = new Cart
            {
                UserId = "TestUser"
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.Sizes.Add(size);
            data.Carts.Add(cart);
            data.SaveChanges();

            var cartsService = new CartService(MapperMock.Instance, data);

            var cartsController = new CartsController(cartsService);
            cartsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var result = cartsController.Clear();

            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void CheckShouldReturnViewWithSum()
        {
            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUser",
                IsPublic = true
            };

            var size = new Size
            {
                Name = "XS"
            };

            var cart = new Cart
            {
                UserId = "TestUser"
            };

            var shirtCart = new ShirtCart
            {
                Cart = cart,
                Shirt = shirt,
                SizeName = size.Name
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.Sizes.Add(size);
            data.Carts.Add(cart);
            data.ShirtCarts.Add(shirtCart);
            data.SaveChanges();

            var cartsService = new CartService(MapperMock.Instance, data);

            var cartsController = new CartsController(cartsService);
            cartsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var result = cartsController.Check();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void BuyShouldReturnRedirectToAction()
        {
            var size = new Size { Name = "XS" };

            var cart = new Cart { UserId = "TestUser" };

            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUser",
                IsPublic = true
            };

            var shirtCart = new ShirtCart
            {
                Cart = cart,
                Shirt = shirt,
                SizeName = size.Name,
                Count = 2
            };

            var shirtSize = new ShirtSize
            {
                Shirt = shirt,
                Size = size,
                Count = 3
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.Sizes.Add(size);
            data.Carts.Add(cart);
            data.ShirtCarts.Add(shirtCart);
            data.ShirtSizes.Add(shirtSize);
            data.SaveChanges();

            var cartsService = new CartService(MapperMock.Instance, data);

            var cartsController = new CartsController(cartsService);
            cartsController.TempData = TempDataMock.Instance;
            cartsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var result = cartsController.Buy();

            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void BuyShouldReturnViewWithProblems()
        {
            var size = new Size { Name = "XS" };

            var cart = new Cart { UserId = "TestUser" };

            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUser",
                IsPublic = true
            };

            var shirtCart = new ShirtCart
            {
                Cart = cart,
                Shirt = shirt,
                SizeName = size.Name,
                Count = 2
            };

            var shirtSize = new ShirtSize
            {
                Shirt = shirt,
                Size = size,
                Count = 1
            };

            var data = DatabaseMock.Instance;
            data.Shirts.Add(shirt);
            data.Sizes.Add(size);
            data.Carts.Add(cart);
            data.ShirtCarts.Add(shirtCart);
            data.ShirtSizes.Add(shirtSize);
            data.SaveChanges();

            var cartsService = new CartService(MapperMock.Instance, data);

            var cartsController = new CartsController(cartsService);
            cartsController.TempData = TempDataMock.Instance;
            cartsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var result = cartsController.Buy();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void BuyShouldReturnBadRequest()
        {
            var data = DatabaseMock.Instance;

            var cartsService = new CartService(MapperMock.Instance, data);

            var cartsController = new CartsController(cartsService);
            cartsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var result = cartsController.Buy();

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
