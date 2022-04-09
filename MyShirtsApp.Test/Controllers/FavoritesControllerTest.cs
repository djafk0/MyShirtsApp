namespace MyShirtsApp.Test.Controllers
{
    using MyShirtsApp.Controllers;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Services.Favorites;
    using MyShirtsApp.Test.Mocks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class FavoritesControllerTest
    {
        [Fact]
        public void AllShouldReturnViewWithFavoriteShirts()
        {
            var user = new User();

            var data = DatabaseMock.Instance;
            data.Users.Add(user);
            data.SaveChanges();

            var favoriteService = new FavoriteService(MapperMock.Instance, data);

            var favoritesController = new FavoritesController(favoriteService);
            favoritesController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var result = favoritesController.All();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ActionShouldReturnOk()
        {
            var user = new User();

            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUserId"
            };

            var data = DatabaseMock.Instance;
            data.Users.Add(user);
            data.Shirts.Add(shirt);
            data.SaveChanges();

            var favoriteService = new FavoriteService(MapperMock.Instance, data);

            var favoritesController = new FavoritesController(favoriteService);
            favoritesController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var result = favoritesController.Action(shirt.Id, shirt.Name);

            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void ActionShouldReturnBadRequest()
        {
            var user = new User();

            var shirt = new Shirt
            {
                Name = "TestName",
                ImageUrl = "TestUrl",
                Price = 30,
                UserId = "TestUserId"
            };

            var data = DatabaseMock.Instance;
            data.Users.Add(user);
            data.Shirts.Add(shirt);
            data.SaveChanges();

            var favoriteService = new FavoriteService(MapperMock.Instance, data);

            var favoritesController = new FavoritesController(favoriteService);
            favoritesController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance(user.Id)
            };

            var result = favoritesController.Action(2, shirt.Name);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
