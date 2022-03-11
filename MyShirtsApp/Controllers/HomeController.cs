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
            var shirts = this.data
               .Shirts
               .OrderByDescending(s => s.SizeId)
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

            return View(shirts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}