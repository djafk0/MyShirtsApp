namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Infrastructure;
    using MyShirtsApp.Services.Favorites;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService favorites;

        public FavoritesController(IFavoriteService favorites)
            => this.favorites = favorites;

        public IActionResult All()
        {
            if (this.User.IsAdmin())
            {
                return RedirectToAction("All", "Shirts");
            }

            var favorites = this.favorites.All(this.User.Id());

            return View(favorites);
        }

        public IActionResult Action(int id, string name)
        {
            if (this.User.IsAdmin())
            {
                return RedirectToAction("All", "Shirts");
            }

            var isAdded = this.favorites
                .IsAdded(id, name, this.User.Id());

            if (!isAdded)
            {
                return BadRequest();
            }

            var url = Request.Headers["Referer"].ToString();

            if (url != string.Empty)
            {
                return Redirect(url);
            }

            return RedirectToAction("All", "Shirts");
        }
    }
}
