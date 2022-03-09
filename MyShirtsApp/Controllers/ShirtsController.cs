namespace MyShirtsApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MyShirtsApp.Data;
    using MyShirtsApp.Models.Shirts;
    using System.Collections.Generic;

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
            if (!ModelState.IsValid)
            {
                return View(shirt);
            }

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
