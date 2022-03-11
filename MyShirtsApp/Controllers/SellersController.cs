namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Models.Sellers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyShirtsApp.Infrastructure;
    using MyShirtsApp.Data.Models;

    public class SellersController : Controller
    {
        private readonly MyShirtsAppDbContext data;

        public SellersController(MyShirtsAppDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Become() 
            => View();

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
                return BadRequest();
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
