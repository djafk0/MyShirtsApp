namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Services.Carts;
    using MyShirtsApp.Infrastructure;
    using MyShirtsApp.Models.Carts;
    using Microsoft.AspNetCore.Mvc;

    public class CartsController : Controller
    {
        private readonly ICartService carts;

        public CartsController(ICartService carts)
            => this.carts = carts;

        public IActionResult Add(int id, [FromQuery]string size)
        {
            var isAdded = this.carts.IsAdded(id, size, this.User.Id());

            if (!isAdded)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Mine));
        }

        public IActionResult Mine()
        {
            var cart = this.carts.MyCart(this.User.Id());

            if (!cart.Any())
            {
                return BadRequest();
            }

            return View(new CartResultViewModel
            {
                Cart = cart,
                TotalPrice = cart.Sum(c => c.Price * c.Count)
            });
        }
    }
}
