using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWRekruter.Data;
using PWRekruter.Models;
using PWRekruter.Services;
using PWRekruter.ViewModels;

namespace PWRekruter.Controllers
{
    public class WiadomosciController : Controller
    {
        private readonly PWRekruterDbContext _context;
        private readonly ILoginService _loginService;

        public WiadomosciController(PWRekruterDbContext context, ILoginService loginService)
        {
            _context = context;
            _loginService = loginService;
        }

        // GET: Wiadomosci
        public async Task<IActionResult> Index()
        {
            // wiadomosci do obecnego uzytkownika
            var wiadomosci = _context.OdbiorcyWiadomosci.Where(ow => ow.OdbiorcaId == _loginService.GetUserId()).Select(w => w.Wiadomosc);

            return View(await wiadomosci.ToListAsync());
        }

        // GET: Wiadomosci/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wiadomosc = await _context.Wiadomosci
                .Include(w => w.Nadawca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wiadomosc == null)
            {
                return NotFound();
            }

            return View(wiadomosc);
        }

        // GET: Wiadomosci/Create
        public IActionResult Create()
        {
            return View();
        }

        // TODO cale filtrowanie kryteriow
        private List<int> GetIdOdbiorcyWiadomosciList(WiadomoscViewModel wiadomoscViewModel)
        {
            var maile = wiadomoscViewModel.Maile.Split(" ");
            return _context.Konta.Where(k => maile.Contains(k.Email)).Select(k=>k.Id).ToList();
        }

        // POST: Wiadomosci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Kierunek,Wydzial,Imie,Nazwisko,Maile,Zweryfikowani,Tytul,Tresc")] WiadomoscViewModel wiadomoscView)
        {
            // TODO wryfikacja kryteriow
            // dla rekrutera wysylanie masowe
            // dla kandydata wysylanie do rekruterow
            if (ModelState.IsValid)
            {
                List<int> ids = GetIdOdbiorcyWiadomosciList(wiadomoscView);

                Wiadomosc wiadomosc = new Wiadomosc
                {
                    NadawcaId = _loginService.GetUserId(),
                    Tytul = wiadomoscView.Tytul,
                    Tresc = wiadomoscView.Tresc,
                    Data = DateTime.Now,
                };
                _context.Add(wiadomosc);

                foreach (int id in ids)
                {
                    OdbiorcaWiadomosci odbiorcaWiadomosci = new OdbiorcaWiadomosci 
                    { 
                        OdbiorcaId = id,
                        Wiadomosc = wiadomosc
                    };

                    _context.Add(odbiorcaWiadomosci);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wiadomoscView);
        }
    }
}
