namespace MyShirtsApp.Controllers
{
    using System.Collections.Generic;
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Models.Shirts;
    using Microsoft.AspNetCore.Mvc;

    public class ShirtsController : Controller
    {
        private readonly MyShirtsAppDbContext data;

        public ShirtsController(MyShirtsAppDbContext data) 
            => this.data = data;

        public IActionResult Add() => View(new AddShirtFormModel
        {
            Sizes = this.GetShirtSizes()
        });

        [HttpPost]
        public IActionResult Add(AddShirtFormModel shirt)
        {
            if (!this.data.Sizes.Any(s => s.Id == shirt.SizeId))
            {
                this.ModelState.AddModelError(nameof(shirt.SizeId), "Size does not exist.");
            }

            if (!ModelState.IsValid)
            {
                shirt.Sizes = this.GetShirtSizes();

                return View(shirt);
            }

            var shirtData = new Shirt
            {
                Name = shirt.Name,
                ImageUrl = shirt.ImageUrl,
                Fabric = shirt.Fabric,
                Price = shirt.Price,
                SizeId = shirt.SizeId
            };

            data.Shirts.Add(shirtData);
            data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult All() => View();

        private IEnumerable<ShirtSizeViewModel> GetShirtSizes()
            => this.data
                .Sizes
                .Select(s => new ShirtSizeViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();
    }
}
