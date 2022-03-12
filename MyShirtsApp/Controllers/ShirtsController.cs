namespace MyShirtsApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Infrastructure;
    using MyShirtsApp.Models.Shirts;
    using System.Collections.Generic;

    public class ShirtsController : Controller
    {
        private readonly MyShirtsAppDbContext data;

        public ShirtsController(MyShirtsAppDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Add()
        {
            if (!this.UserIsSeller())
            {
                return RedirectToAction(nameof(SellersController.Become), "Sellers");
            }

            return View(new AddShirtFormModel
            {

                Sizes = this.GetShirtSizes()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddShirtFormModel shirt)
        {
            var sellerId = this.data
                .Sellers
                .Where(s => s.UserId == this.User.GetId())
                .Select(s => s.Id)
                .FirstOrDefault();

            if (sellerId == 0)
            {
                return RedirectToAction(nameof(SellersController.Become), "Sellers");
            }

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
                SizeId = shirt.SizeId,
                SellerId = sellerId
            };

            this.data.Shirts.Add(shirtData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery] AllShirtsQueryModel query)
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

            var totalShirts = shirtsQuery.Count();

            if (query.CurrentPage < 1)
            {
                query.CurrentPage = 1;
            }

            var shirts = shirtsQuery
                .Skip((query.CurrentPage - 1) * AllShirtsQueryModel.ShirtsPerPage)
                .Take(AllShirtsQueryModel.ShirtsPerPage)
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

            query.TotalShirts = totalShirts;
            query.Shirts = shirts;
            query.Sizes = shirtSizes;

            return View(query);
        }

        private bool UserIsSeller()
            => this.data
                .Sellers
                .Any(s => s.UserId == this.User.GetId());

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
