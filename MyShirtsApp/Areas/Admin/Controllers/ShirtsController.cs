namespace MyShirtsApp.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ShirtsController : AdminController
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
