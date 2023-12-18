using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PWRekruter.Data;
using System;
using System.Diagnostics;
using System.Linq;

namespace PWRekruter.Controllers
{
    public class LogowanieController : Controller
    {
        private readonly PWRekruterDbContext _context;

        public LogowanieController(PWRekruterDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            ViewData["Kandydaci"] = _context.Kandydaci.ToList();
            ViewData["Rekruterzy"] = _context.Rekruterzy.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Kandydat(int id)
        {
            Debug.WriteLine($"Kandydat {id}");
            Response.Cookies.Append("UserType", "kandydat");
            Response.Cookies.Append("UserId", id.ToString());

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rekruter(int id)
        {
            Debug.WriteLine($"Rekruter {id}");
            Response.Cookies.Append("UserType", "rekruter");
            Response.Cookies.Append("UserId", id.ToString());

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserType");
            Response.Cookies.Delete("UserId");

            return RedirectToAction("Index", "Home");
        }
    }
}
