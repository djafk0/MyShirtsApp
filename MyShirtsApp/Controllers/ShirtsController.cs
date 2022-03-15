namespace MyShirtsApp.Controllers
{
    using System.Collections.Generic;
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ShirtsController : Controller
    {
        private readonly IShirtService shirts;
        private readonly MyShirtsAppDbContext data;

        public ShirtsController(IShirtService shirts, MyShirtsAppDbContext data)
        {
            this.shirts = shirts;
            this.data = data;
        }

        [Authorize]
        public IActionResult Add() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddShirtFormModel shirt)
        {
            //if (!this.data.Sizes.Any(s => s.Id == shirt.SizeId))
            //{
            //    this.ModelState.AddModelError(nameof(shirt.SizeId), "Size does not exist.");
            //}

            if (!ModelState.IsValid)
            { 
                return View(shirt);
            }

            var shirtData = new Shirt
            {
                Name = shirt.Name,
                ImageUrl = shirt.ImageUrl,
                Price = shirt.Price,
            };

            List<int> sizes = new List<int>()
            {
                shirt.SizeXS,
                shirt.SizeS,
                shirt.SizeM,
                shirt.SizeL,
                shirt.SizeXL,
                shirt.SizeXXL
            };

            for (int i = 1; i <= 6; i++)
            {
                shirtData.ShirtSizes.Add(new ShirtSize { SizeId = i, Count = sizes[i - 1] });
            }

            this.data.Shirts.Add(shirtData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery] AllShirtsQueryModel query)
        {
            var queryResult = this.shirts.All(
                query.SizeId,
                query.Sorting,
                query.CurrentPage,
                AllShirtsQueryModel.ShirtsPerPage);

            //var shirtSizes = this.shirts.AllShirtSizes();

            query.TotalShirts = queryResult.TotalShirts;
            query.Shirts = queryResult.Shirts;
            //query.Sizes = shirtSizes;

            return View(query);
        }

        //private IEnumerable<ShirtSizeViewModel> GetShirtSizes()
        //    => this.data
        //        .Sizes
        //        .Select(s => new ShirtSizeViewModel
        //        {
        //            Id = s.Id,
        //            Name = s.Name
        //        })
        //        .ToList();
    }
}
