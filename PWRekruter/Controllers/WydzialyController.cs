using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWRekruter.Data;
using PWRekruter.Models;

namespace PWRekruter.Controllers
{
    public class WydzialyController : Controller
    {
        private readonly PWRekruterDbContext _context;

        public WydzialyController(PWRekruterDbContext context)
        {
            _context = context;
        }

        // GET: Wydzialy
        public async Task<IActionResult> Index()
        {
            return View(await _context.Wydzialy.ToListAsync());
        }

        // GET: Wydzialy/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wydzial = await _context.Wydzialy
                .FirstOrDefaultAsync(m => m.Symbol == id);
            if (wydzial == null)
            {
                return NotFound();
            }
            wydzial.Kierunki = await _context.Kierunki.Where(k => k.SymbolWydzialu == wydzial.Symbol).ToListAsync();
            return View(wydzial);
        }

        private bool WydzialExists(string id)
        {
            return _context.Wydzialy.Any(e => e.Symbol == id);
        }
    }
}
