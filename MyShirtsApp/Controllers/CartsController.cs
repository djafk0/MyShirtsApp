namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Services.Carts;
    using Microsoft.AspNetCore.Mvc;
    using MyShirtsApp.Infrastructure;

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

            return View();
        }
    }
}
