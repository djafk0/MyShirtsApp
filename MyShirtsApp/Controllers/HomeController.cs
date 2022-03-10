﻿namespace MyShirtsApp.Controllers
{
    using System.Diagnostics;
    using MyShirtsApp.Models;
    using Microsoft.AspNetCore.Mvc;
    using MyShirtsApp.Data;
    using MyShirtsApp.Models.Shirts;

    public class HomeController : Controller
    {
        private readonly MyShirtsAppDbContext data;

        public HomeController(MyShirtsAppDbContext data)
            => this.data = data;

        public IActionResult Index()
        {
            var shirts = this.data
               .Shirts
               .OrderByDescending(s => s.SizeId)
               .Take(3)
               .Select(s => new ShirtListingViewModel
               {
                   Id = s.Id,
                   Name = s.Name,
                   Fabric = s.Fabric,
                   ImageUrl = s.ImageUrl,
                   Price = s.Price,
                   Size = s.Size.Name
               })
               .ToList();

            return View(shirts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}