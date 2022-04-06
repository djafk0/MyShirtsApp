namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Infrastructure;
    using MyShirtsApp.Services.Users;
    using MyShirtsApp.Services.Users.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;

    [Authorize(Roles = UserRole)]
    public class UsersController : Controller
    {
        private readonly IUserService users;

        public UsersController(IUserService users)
            => this.users = users;

        public IActionResult Become()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Become(BecomeSellerFormModel user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            this.users.BecomeSeller(this.User.Id(), user.CompanyName);

            TempData[GlobalMessageKey] = "Thank you for becomming a seller !";

            return RedirectToAction("Mine", "Shirts");
        }
    }
}
