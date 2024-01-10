using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PWRekruter.Data;
using PWRekruter.Services;
using System;
using System.Diagnostics;
using System.Linq;

namespace PWRekruter.Controllers
{
    public class LogowanieController : Controller
    {
        private readonly PWRekruterDbContext _context;
        private readonly ILoginService _loginService;

        public LogowanieController(PWRekruterDbContext context, ILoginService loginService)
        {
            _context = context;
            _loginService = loginService;
        }

        public IActionResult Login()
        {
            ViewData["Kandydaci"] = _context.Kandydaci.ToList();
            ViewData["Rekruterzy"] = _context.Rekruterzy.ToList();
            return View();
        }


        // to do po nie zaznaczeniu który rekruter/kantydat i kliknieciu zaloguj wywala 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Kandydat(int id)
        {
            _loginService.Login(id, UserType.Kandydat);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rekruter(int id)
        {
            _loginService.Login(id, UserType.Rekruter);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            _loginService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
