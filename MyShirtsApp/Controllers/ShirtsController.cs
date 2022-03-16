namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Data;
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
        public IActionResult Add()
        {
            return View(new AddShirtFormModel
            {
                IsValidSize = true,
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddShirtFormModel shirt)
        {
            var sizes = this.shirts.GetSizes(shirt);

            if (sizes.All(x => x == 0))
            {
                this.ModelState.AddModelError(nameof(shirt.IsValidSize), "Please fill at least one field");
                shirt.IsValidSize = false;
            }

            if (!ModelState.IsValid)
            {
                return View(shirt);
            }

            this.shirts.Create(
                shirt.Name, 
                shirt.ImageUrl, 
                shirt.Price, 
                sizes);

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery] AllShirtsQueryModel query)
        {
            var queryResult = this.shirts.All(
                query.Size,
                query.Sorting,
                query.CurrentPage,
                AllShirtsQueryModel.ShirtsPerPage);

            query.TotalShirts = queryResult.TotalShirts;
            query.Shirts = queryResult.Shirts;

            return View(query);
        }
    }
}
