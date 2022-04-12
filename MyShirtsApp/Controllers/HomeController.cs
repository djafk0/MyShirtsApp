namespace MyShirtsApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MyShirtsApp.Services.Shirts;

    public class HomeController : Controller
    {
        private readonly IShirtService shirts;

        public HomeController(IShirtService shirts) 
            => this.shirts = shirts;

        public IActionResult Index()
        {
            var shirts = this.shirts.Latest();

            return View(shirts);
        }

        public IActionResult Error() => View();
    }
}