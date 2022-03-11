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

            this.data.Shirts.Add(shirtData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery]AllShirtsQueryModel query)
        {
            var shirtsQuery = this.data
                .Shirts
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Size))
            {
                shirtsQuery = shirtsQuery.Where(s => s.Size.Name == query.Size);
            }

            shirtsQuery = query.Sorting switch
            {
                ShirtSorting.Newest => shirtsQuery.OrderByDescending(s => s.Id),
                ShirtSorting.Oldest => shirtsQuery.OrderBy(s => s.Id),
                ShirtSorting.PriceAsc => shirtsQuery.OrderBy(s => s.Price),
                ShirtSorting.PriceDesc or _ => shirtsQuery.OrderByDescending(s => s.Price)
            };

            var shirts = shirtsQuery
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

            var shirtSizes = this.data
                .Sizes
                .Select(s => s.Name)
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            query.Shirts = shirts;
            query.Sizes = shirtSizes;

            return View(query);
        }

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
