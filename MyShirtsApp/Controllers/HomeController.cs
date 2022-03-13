namespace MyShirtsApp.Controllers
{
    using System.Diagnostics;
    using MyShirtsApp.Data;
    using MyShirtsApp.Models;
    using MyShirtsApp.Models.Shirts;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly MyShirtsAppDbContext data;

        public HomeController(MyShirtsAppDbContext data)
            => this.data = data;

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}