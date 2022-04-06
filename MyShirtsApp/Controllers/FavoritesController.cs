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
            var favorites = this.favorites.All(this.User.Id());

            return View(favorites);
        }

        public IActionResult Action(int id, string name)
        {
            var isAdded = this.favorites
                .IsAdded(id, name, this.User.Id());

            if (!isAdded)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
