namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts;
    using MyShirtsApp.Infrastructure;
    using MyShirtsApp.Services.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    public class ShirtsController : Controller
    {
        private readonly IShirtService shirts;
        private readonly IUserService users;
        private readonly IMapper mapper;

        public ShirtsController(
            IShirtService shirts,
            IUserService users,
            IMapper mapper)
        {
            this.shirts = shirts;
            this.users = users;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult Add()
        {
            var isSeller = this.users.IsSeller(this.User.Id());

            if (!isSeller)
            {
                return RedirectToAction("Become", "Users");
            }

            if (this.User.IsAdmin())
            {
                return RedirectToAction(nameof(All));
            }

            return View(new ShirtFormModel
            {
                IsValidSize = true,
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(ShirtFormModel shirt)
        {
            var isSeller = this.users.IsSeller(this.User.Id());

            if (!isSeller)
            {
                return RedirectToAction("Become", "Users");
            }

            if (this.User.IsAdmin())
            {
                return RedirectToAction(nameof(All));
            }

            var sizes = this.shirts.SizesFromModel(shirt);

            shirt.IsValidSize = true;

            if (sizes.All(x => x == 0))
            {
                this.ModelState.AddModelError(string.Empty, string.Empty);

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
            var isSeller = this.users.IsSeller(this.User.Id());

            if (!isSeller)
            {
                return RedirectToAction("Become", "Users");
            }

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

            if (shirt.UserId != this.User.Id())
            {
                return Unauthorized();
            }

            var shirtForm = this.mapper
                .Map<ShirtFormModel>(shirt);

            shirtForm.IsValidSize = true;

            return View(shirtForm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, ShirtFormModel shirt)
        {
            var shirtDetails = this.shirts.Details(id);

            if (shirtDetails.UserId != this.User.Id())
            {
                return Unauthorized();
            }

            var sizes = this.shirts.SizesFromModel(shirt);

            if (sizes.All(x => x == 0))
            {
                this.ModelState.AddModelError(string.Empty, string.Empty);

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

            if (shirt == null)
            {
                return BadRequest();
            }

            return View(shirt);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var isDeleted = this.shirts
                .Delete(
                    id,
                    this.User.Id(),
                    this.User.IsAdmin());

            if (!isDeleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Mine));
        }
    }
}