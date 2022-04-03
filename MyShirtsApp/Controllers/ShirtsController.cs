namespace MyShirtsApp.Controllers
{
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts;
    using MyShirtsApp.Infrastructure;
    using MyShirtsApp.Services.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;

    using static WebConstants;

    [Authorize]
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

            var shirtId = this.shirts.Create(
                shirt.Name,
                shirt.ImageUrl,
                shirt.Price,
                this.User.Id(),
                sizes);

            TempData[GlobalMessageKey] = "Your shirt was added successfully and it is awaiting to be approved.";

            return RedirectToAction(nameof(Details), new { id = shirtId, name = shirt.Name });
        }

        [AllowAnonymous]
        public IActionResult All([FromQuery] AllShirtsQueryModel query)
        {
            string userId = null;

            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.User.Id();
            }

            var queryResult = this.shirts.All(
                query.Size,
                query.Sorting,
                query.CurrentPage,
                query.ShirtsPerPage,
                userId);

            query.TotalShirts = queryResult.TotalShirts;
            query.Shirts = queryResult.Shirts;

            return View(query);
        }

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

        public IActionResult Edit(int id)
        {
            var shirt = this.shirts.Details(id);

            if (shirt == null)
            {
                return BadRequest();
            }

            if (shirt.UserId != this.User.Id())
            {
                return Unauthorized();
            }

            var shirtForm = this.mapper
                .Map<ShirtFormModel>(shirt);

            shirtForm.IsValidSize = true;

            return View(shirtForm);
        }

        [HttpPost]
        public IActionResult Edit(int id, ShirtFormModel shirt)
        {
            var userId = this.User.Id();

            var isAdmin = this.User.IsAdmin();

            var shirtDetails = this.shirts.Details(id);

            if (shirtDetails == null)
            {
                return RedirectToAction(nameof(All));
            }

            if (shirtDetails.UserId != userId)
            {
                return RedirectToAction(nameof(All));
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
                userId,
                isAdmin,
                sizes);

            if (!isShirtEdited)
            {
                return RedirectToAction(nameof(All));
            }

            TempData[GlobalMessageKey] = "Your shirt was edited successfully and it is awaiting to be approved.";

            return RedirectToAction(nameof(Details), new { id, name = shirt.Name });
        }

        [AllowAnonymous]
        public IActionResult Details(int id, string name)
        {
            var shirt = this.shirts.Details(id);

            if (shirt == null)
            {
                return BadRequest();
            }

            name = name.Replace("-", " ");

            if (shirt.Name != name)
            {
                return BadRequest();
            }

            return View(shirt);
        }

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