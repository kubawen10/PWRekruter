using Microsoft.AspNetCore.Mvc;
using PWRekruter.Services;
using System.Diagnostics;

namespace PWRekruter.Controllers
{
    public class KontaController : Controller
    {
        private readonly ILoginService _loginService;
        public KontaController(ILoginService loginService) 
        {
            _loginService = loginService;
        }

        public IActionResult Index()
        {
            if (_loginService.GetUserType() == UserType.Kandydat)
            {
                return RedirectToAction("Index", "Kandydaci");
            } else // TODO redirect dla rekrutera
            {
                return RedirectToAction("Index", "Rekruterzy");
            }
        }
    }
}
