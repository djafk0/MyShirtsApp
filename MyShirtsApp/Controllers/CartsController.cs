﻿namespace MyShirtsApp.Controllers
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

        public IActionResult Add(int id, [FromQuery] string size)
        {
            if (this.User.IsAdmin())
            {
                return RedirectToAction("All", "Shirts");
            }

            var isAdded = this.carts.IsAdded(id, size, this.User.Id());

            if (!isAdded)
            {
                return BadRequest();
            }

            return RedirectToAction("All", "Shirts");
        }

        public IActionResult Mine()
        {
            if (this.User.IsAdmin())
            {
                return RedirectToAction("All", "Shirts");
            }

            var cart = this.carts.MyCart(this.User.Id());

            return View(new CartResultViewModel
            {
                Cart = cart,
                TotalPrice = cart.Sum(c => c.Price * c.Count)
            });
        }

        public IActionResult DeleteShirt([FromQuery] CartQueryViewModel query)
        {
            var isDeleted = this.carts.IsDeletedShirt(
                query.ShirtId, 
                this.User.Id(),
                query.SizeName,
                query.Flag);

            if (!isDeleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Mine));
        }

        public IActionResult Clear()
        {
            this.carts.ClearCart(this.User.Id());

            return RedirectToAction(nameof(Mine));
        }

        public IActionResult Buy()
        {
            var problems = this.carts.BuyAll(this.User.Id());

            if (problems.Any())
            {
                return View(problems);
            }

            this.carts.ClearCart(this.User.Id());

            return RedirectToAction("All", "Shirts");
        }
    }
}
