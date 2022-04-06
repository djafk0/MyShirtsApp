namespace MyShirtsApp.Areas.Admin.Controllers
{
    using MyShirtsApp.Services.Shirts;
    using Microsoft.AspNetCore.Mvc;

    public class ShirtsController : AdminController
    {
        private readonly IShirtService shirts;

        public ShirtsController(IShirtService shirts) 
            => this.shirts = shirts;

        public IActionResult All()
        {
            var shirts = this.shirts
                .All(publicOnly: false)
                .Shirts;

            return View(shirts);
        }

        public IActionResult ChangeVisibility(int id)
        {
            var isChanged = this.shirts.ChangeVisibility(id);

            if(!isChanged)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
