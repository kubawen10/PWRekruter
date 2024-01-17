using Microsoft.AspNetCore.Mvc;

namespace PWRekruter.Controllers
{
    public class RekruterzyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
