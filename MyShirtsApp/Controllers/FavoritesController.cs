namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Infrastructure;
    using MyShirtsApp.Services.Favorites;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    [Authorize(Roles = "User, Seller")]
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService favorites;

        public FavoritesController(IFavoriteService favorites)
            => this.favorites = favorites;

        public IActionResult All()
        {
            var userId = this.User.Id();

            var favorites = this.favorites.All(userId);

            return View(favorites);
        }

        public IActionResult Action(int id, string name)
        {
            var userId = this.User.Id();

            var isAdded = this.favorites
                .IsAdded(id, name, userId);

            if (!isAdded)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
