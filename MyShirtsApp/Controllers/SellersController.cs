namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Models.Sellers;
    using MyShirtsApp.Infrastructure;
    using MyShirtsApp.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class SellersController : Controller
    {
        private readonly MyShirtsAppDbContext data;

        public SellersController(MyShirtsAppDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Become()
        {
            var userId = this.User.GetId();

            var userIsAlreadySeller = this.data
                .Sellers
                .Any(s => s.UserId == userId);

            if (userIsAlreadySeller)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeSellerFormModel seller)
        {
            var userId = this.User.GetId();

            var userIsAlreadySeller = this.data
                .Sellers
                .Any(s => s.UserId == userId);

            if (userIsAlreadySeller)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(seller);
            }

            var sellerData = new Seller
            {
                Name = seller.Name,
                PhoneNumber = seller.PhoneNumber,
                UserId = userId,
            };

            this.data.Sellers.Add(sellerData);
            this.data.SaveChanges();

            return RedirectToAction("All", "Shirts");
        }
    }
}
