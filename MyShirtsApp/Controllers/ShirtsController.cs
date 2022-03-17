namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyShirtsApp.Infrastructure;

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
            return View(new ShirtFormModel
            {
                IsValidSize = true,
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(ShirtFormModel shirt)
        {
            var sizes = this.shirts.GetSizesFromModel(shirt);
            var isSizesValid = false;

            if (sizes.All(x => x == 0))
            {
                isSizesValid = true;
                this.ModelState.AddModelError(nameof(shirt.IsValidSize), "Please fill at least one field");
                shirt.IsValidSize = false;
            }

            if (!ModelState.IsValid)
            {
                if (!isSizesValid)
                {
                    shirt.IsValidSize = true;
                }

                return View(shirt);
            }

            this.shirts.Create(
                shirt.Name,
                shirt.ImageUrl,
                shirt.Price,
                this.User.GetId(),
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

        [Authorize]
        public IActionResult Mine()
        {
            var myShirts = this.shirts.GetShirtsByUser(this.User.GetId());

            return View(myShirts);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var shirt = this.shirts.Details(id);

            if (shirt.UserId != this.User.GetId())
            {
                return Unauthorized();
            }

            return View(new ShirtFormModel
            {
                Name = shirt.Name,
                ImageUrl = shirt.ImageUrl,
                Price = shirt.Price,
                SizeXS = shirt.SizeXS,
                SizeS = shirt.SizeS,
                SizeM = shirt.SizeM,
                SizeL = shirt.SizeL,
                SizeXL = shirt.SizeXL,
                SizeXXL = shirt.SizeXXL,
                IsValidSize = true
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, ShirtFormModel shirt)
        {
            var sizes = this.shirts.GetSizesFromModel(shirt);

            if (sizes.All(x => x == 0))
            {
                this.ModelState.AddModelError(nameof(shirt.IsValidSize), "Please fill at least one field");
                shirt.IsValidSize = false;
            }

            if (!ModelState.IsValid)
            {
                return View(shirt);
            }

            var isShirtEdited = this.shirts.Edit(
                id,
                shirt.Name,
                shirt.ImageUrl,
                shirt.Price,
                this.User.GetId(),
                sizes);

            if (!isShirtEdited)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }
    }
}