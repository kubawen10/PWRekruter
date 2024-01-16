using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var userId = _loginService.GetUserId();
            // wiadomosci do obecnego uzytkownika
            var wiadomosci = _context.OdbiorcyWiadomosci
                .Include(w => w.Wiadomosc.Nadawca)
                .Where(ow => ow.OdbiorcaId == userId)
                .Select(w => w.Wiadomosc)
                .ToListAsync();

            return View(await wiadomosci);
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
        public async Task<IActionResult> Create()
        {
            var userType = _loginService.GetUserType();
            if (userType == UserType.Kandydat)
            {
                ViewBag.UserType = "Kandydat";
                return View();
            }

            ViewBag.UserType = "Rekruter";

			var wydzialy = await _context.Wydzialy.ToListAsync();
			ViewBag.Wydzialy = wydzialy;

            var kierunki = await _context.Kierunki.ToListAsync();
            ViewBag.Kierunki = kierunki;

			return View();
        }

        private async Task<List<int>> GetIdOdbiorcyWiadomosciList(WiadomoscViewModel wiadomoscViewModel)
        {
            // user==Kandydat => mail do pierwszego rekrutera
            var userType = _loginService.GetUserType();
            if (userType == UserType.Kandydat)
            {
                var rekruterId = await _context.Rekruterzy.Select(r=>r.Id).FirstOrDefaultAsync();
                return new List<int> { rekruterId };
            }

            if(wiadomoscViewModel.PusteDaneOdbiorcy())
            {
                return new List<int>();
            }

            string[] maile = new string[0];
            if (!string.IsNullOrEmpty(wiadomoscViewModel.Maile))
            {
                maile = wiadomoscViewModel.Maile.Split(" ");
            }

            // TODO brakuje weryfikacji czy zakwalifikowany ale nie wiem czy bedziemy to implementowac
            // https://media.tenor.com/vkYnJE2Jdj8AAAAM/oh-my-god.gif
            var ids = await _context.Preferencje
                .Include(p => p.Aplikacja)
                    .ThenInclude(a => a.Kandydat)
                .Include(p => p.Kierunek)
                    .ThenInclude(k => k.Wydzial)
                .Where(p => (string.IsNullOrEmpty(wiadomoscViewModel.Wydzial) || p.Kierunek.Wydzial.Symbol == wiadomoscViewModel.Wydzial)
                    && (string.IsNullOrEmpty(wiadomoscViewModel.Kierunek) || p.Kierunek.Skrot == wiadomoscViewModel.Kierunek)
                    && (string.IsNullOrEmpty(wiadomoscViewModel.Maile) || maile.Contains(p.Aplikacja.Kandydat.Email))
                    && (string.IsNullOrEmpty(wiadomoscViewModel.Imie) || p.Aplikacja.Kandydat.Imie == wiadomoscViewModel.Imie)
                    && (string.IsNullOrEmpty(wiadomoscViewModel.Nazwisko) || p.Aplikacja.Kandydat.Nazwisko == wiadomoscViewModel.Nazwisko))
                .Select(p => p.Aplikacja.Kandydat.Id)
                .Distinct()
                .ToListAsync();

            return ids;
        }

        // POST: Wiadomosci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Kierunek,Wydzial,Imie,Nazwisko,Maile,Zakwalifikowani,Tytul,Tresc")] WiadomoscViewModel wiadomoscView)
        {
            if (!ModelState.IsValid)
            {
                return View(wiadomoscView);
            }

            List<int> idOdbiorcow = await GetIdOdbiorcyWiadomosciList(wiadomoscView);

            if (idOdbiorcow.Count == 0)
            {
                ViewBag.BrakKandydatow = true;
                ViewBag.UserType = "Rekruter";

                var wydzialy = await _context.Wydzialy.ToListAsync();
                var kierunki = await _context.Kierunki.ToListAsync();
                ViewBag.Wydzialy = wydzialy;
                ViewBag.Kierunki = kierunki;

                return View(wiadomoscView);
            }

            Wiadomosc wiadomosc = new Wiadomosc
            {
                NadawcaId = _loginService.GetUserId(),
                Tytul = wiadomoscView.Tytul,
                Tresc = wiadomoscView.Tresc,
                Data = DateTime.Now,
                Odbiorcy = new Collection<OdbiorcaWiadomosci>()
            };

            foreach (int id in idOdbiorcow)
            {
                OdbiorcaWiadomosci odbiorcaWiadomosci = new OdbiorcaWiadomosci
                {
                    OdbiorcaId = id,
                    Wiadomosc = wiadomosc
                };

                _context.Add(odbiorcaWiadomosci);
            }
            _context.Add(wiadomosc);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
