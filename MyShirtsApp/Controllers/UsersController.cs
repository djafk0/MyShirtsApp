﻿namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Infrastructure;
    using MyShirtsApp.Services.Users;
    using MyShirtsApp.Services.Users.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;

    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService users;

        public UsersController(IUserService users)
            => this.users = users;

        public IActionResult Become()
        {
            if (this.users.IsSeller(this.User.Id()) || this.User.IsAdmin())
            {
                return RedirectToAction("All", "Shirts");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Become(BecomeSellerFormModel user)
        {
            if (this.users.IsSeller(this.User.Id()) || this.User.IsAdmin())
            {
                return RedirectToAction("All", "Shirts");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            this.users.BecomeSeller(this.User.Id(), user.CompanyName);

            TempData[GlobalSuccessMessageKey] = "Thank you for becomming a seller!";

            return RedirectToAction("All", "Shirts");
        }
    }
}