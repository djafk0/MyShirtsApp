namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Services.Carts;
    using MyShirtsApp.Infrastructure;
    using MyShirtsApp.Models.Carts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using static WebConstants;

    [Authorize(Roles = "User, Seller")]
    public class CartsController : Controller
    {
        private readonly ICartService carts;

        public CartsController(ICartService carts)
            => this.carts = carts;

        public IActionResult Add(int id, [FromQuery] string size)
        {
            var userId = this.User.Id();

            var isAdded = this.carts.IsAdded(id, size, userId);

            if (!isAdded)
            {
                return BadRequest();
            }

            return Ok();
        }

        public IActionResult Mine()
        {
            var userId = this.User.Id();

            var cart = this.carts.MyCart(userId);

            return View(new CartResultViewModel
            {
                Cart = cart,
                TotalPrice = cart.Sum(c => c.Price * c.Count)
            });
        }

        public IActionResult DeleteShirt([FromQuery] CartQueryViewModel query)
        {
            var userId = this.User.Id();

            var isDeleted = this.carts
                .IsDeletedShirt(
                    userId,
                    query.ShirtId,
                    query.SizeName,
                    query.Flag);

            if (!isDeleted)
            {
                return BadRequest();
            }

            return Ok();
        }

        public IActionResult Clear()
        {
            var userId = this.User.Id();

            this.carts.ClearCart(userId);

            return Ok();
        }

        public IActionResult Check()
        {
            var userId = this.User.Id();

            var cart = this.carts.MyCart(userId);

            return View(new CartResultViewModel
            {
                TotalPrice = cart.Sum(c => c.Price * c.Count)
            });
        }

        public IActionResult Buy()
        {
            var userId = this.User.Id();

            var problems = this.carts.BuyAll(userId);

            if (problems == null)
            {
                return BadRequest();
            }

            if (problems.Any())
            {
                return View("Problems", problems);
            }

            this.carts.ClearCart(userId);

            TempData[GlobalMessageKey] = "Your purchase was successful !";

            return RedirectToAction("All", "Shirts");
        }
    }
}
