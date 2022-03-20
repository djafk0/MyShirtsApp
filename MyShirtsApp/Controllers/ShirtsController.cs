namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts;
    using MyShirtsApp.Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ShirtsController : Controller
    {
        private readonly IShirtService shirts;

        public ShirtsController(IShirtService shirts)
        {
            this.shirts = shirts;
        }

        [Authorize]
        public IActionResult Add()
        {
            return View(new ShirtFormModel
            {
                Sizes = this.shirts.GetSizesAsModel().ToList(),
                IsValidSize = true,
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(ShirtFormModel shirt)
        {
            var sizes = this.shirts.SizesFromModel(shirt);

<<<<<<< HEAD
            shirt.IsValidSize = true;

            if (sizes.All(x => x == 0))
            {
=======
            var isSizesValid = false;

            if (sizes.All(x => x == 0))
            {
                isSizesValid = true;

                this.ModelState.AddModelError(nameof(shirt.IsValidSize), "Please fill at least one field");

>>>>>>> 5a5a3411f73db63a68a3c3d17448d4ee1ae7200b
                shirt.IsValidSize = false;

                this.ModelState.AddModelError(nameof(shirt.IsValidSize), "Please fill at least one field");
            }

            if (!ModelState.IsValid)
            {
<<<<<<< HEAD
=======
                if (!isSizesValid)
                {
                    shirt.IsValidSize = true;
                    shirt.Sizes = this.shirts.GetSizesAsModel().ToList();
                }

>>>>>>> 5a5a3411f73db63a68a3c3d17448d4ee1ae7200b
                return View(shirt);
            }

            this.shirts.Create(
                shirt.Name,
                shirt.ImageUrl,
                shirt.Price,
                this.User.Id(),
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
            if (this.User.IsAdmin())
            {
                return RedirectToAction(nameof(All));
            }

            var myShirts = this.shirts.ShirtsByUser(this.User.Id());

            return View(myShirts);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var shirt = this.shirts.Details(id);

            if (shirt.UserId != this.User.Id() && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var sizes = shirt.Sizes.ToList();

            return View(new ShirtFormModel
            {
                Name = shirt.Name,
                ImageUrl = shirt.ImageUrl,
                Price = shirt.Price,
                Sizes = sizes,
                IsValidSize = true
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, ShirtFormModel shirt)
        {
            var sizes = this.shirts.SizesFromModel(shirt);

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
                this.User.Id(),
                this.User.IsAdmin(),
                sizes);

            if (!isShirtEdited)
            {
                return Unauthorized();
            }

            return RedirectToAction(nameof(Mine));
        }

        public IActionResult Details(int id)
        {
            var shirt = this.shirts.Details(id);

            return View(shirt);
        }

        public IActionResult Delete(int id)
        {
            var userId = this.User.Id();
            var isAdmin = this.User.IsAdmin();

            var isDeleted = this.shirts
                .Delete(id, userId, isAdmin);

            if (!isDeleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Mine));
        }
    }
}