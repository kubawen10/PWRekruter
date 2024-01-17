﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PWRekruter.Data;
using PWRekruter.Enums;
using PWRekruter.Models;
using PWRekruter.Services;
using PWRekruter.ViewModels;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PWRekruter.DTO;
using System.IO;
using System.Net.Http;
using System.Text;

namespace PWRekruter.Controllers
{
    public class KandydaciController : Controller
    {
        private readonly PWRekruterDbContext _context;
        private readonly ILoginService _loginService;

        public KandydaciController(PWRekruterDbContext context, ILoginService loginService)
        {
            _context = context;
            _loginService = loginService;
        }

        // GET: Kandydaci
        public async Task<IActionResult> Index()
        {
            int kandydatId = _loginService.GetUserId();
            var kandydat = await _context.Kandydaci.FindAsync(kandydatId);
            if (kandydat == null)
            {
                return NotFound();
            }
            return View(kandydat);
        }

        // GET: Kandydaci/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kandydat = await _context.Kandydaci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kandydat == null)
            {
                return NotFound();
            }

            return View(kandydat);
        }

        // GET: Kandydaci/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kandydaci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Imie,DrugieImie,Nazwisko,Pesel,Plec,DataUrodzenia,Panstwo,KodPocztowy,Miejscowosc,Ulica,NumerBudynku,NumerMieszkania,Id,Email,Haslo")] Kandydat kandydat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kandydat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kandydat);
        }

        // GET: Kandydaci/Edit/5
        public async Task<IActionResult> Edit()
        {
            int kandydatId = _loginService.GetUserId();

            var kandydat = await _context.Kandydaci.FindAsync(kandydatId);
            if (kandydat == null)
            {
                return NotFound();
            }

            KandydatViewModel kandydatViewModel = new KandydatViewModel
            {
                Imie = kandydat.Imie,
                DrugieImie = kandydat.DrugieImie,
                Nazwisko = kandydat.Nazwisko,
                Pesel = kandydat.Pesel,
                Plec = kandydat.Plec,
                DataUrodzenia = kandydat.DataUrodzenia,
                Panstwo = kandydat.Panstwo,
                KodPocztowy = kandydat.KodPocztowy,
                Miejscowosc = kandydat.Miejscowosc,
                Ulica = kandydat.Ulica,
                NumerBudynku = kandydat.NumerBudynku,
                NumerMieszkania = kandydat.NumerMieszkania
            };
            return View(kandydatViewModel);
        }

        // POST: Kandydaci/Edit/
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Imie,DrugieImie,Nazwisko,Pesel,Plec,DataUrodzenia,Panstwo,KodPocztowy,Miejscowosc,Ulica,NumerBudynku,NumerMieszkania")] KandydatViewModel kandydatView)
        {
            if (ModelState.IsValid)
            {
                var kandydatId = _loginService.GetUserId();
                var kandydat = await _context.Kandydaci.FindAsync(kandydatId);
                if (kandydat == null)
                {
                    return NotFound();
                }

                kandydat.Imie = kandydatView.Imie;
                kandydat.DrugieImie = kandydatView.DrugieImie;
                kandydat.Nazwisko = kandydatView.Nazwisko;
                kandydat.Pesel = kandydatView.Pesel;
                kandydat.Plec = kandydatView.Plec;
                kandydat.DataUrodzenia = kandydatView.DataUrodzenia;
                kandydat.Panstwo = kandydatView.Panstwo;
                kandydat.KodPocztowy = kandydatView.KodPocztowy;
                kandydat.Miejscowosc = kandydatView.Miejscowosc;
                kandydat.Ulica = kandydatView.Ulica;
                kandydat.NumerBudynku = kandydatView.NumerBudynku;
                kandydat.NumerMieszkania = kandydatView.NumerMieszkania;

                _context.Update(kandydat);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(kandydatView);
        }

        // GET: Kandydaci/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kandydat = await _context.Kandydaci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kandydat == null)
            {
                return NotFound();
            }

            return View(kandydat);
        }

        // POST: Kandydaci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kandydat = await _context.Kandydaci.FindAsync(id);
            _context.Kandydaci.Remove(kandydat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KandydatExists(int id)
        {
            return _context.Kandydaci.Any(e => e.Id == id);
        }

        // GET: Kandydaci/Aplikacja
        public async Task<IActionResult> Aplikacja()
        {
            int kandydatId = _loginService.GetUserId();
            Aplikacja aplikacja = await _context.Aplikacje
                .FirstOrDefaultAsync(aplikacja => aplikacja.IdKandydata == kandydatId);

            if (aplikacja == null)
            {
                return View();
            }
            aplikacja.TuraRekrutacji = await _context.TuryRekrutacji
                                       .Where(tura => tura.Id == aplikacja.IdTuryRekrutacji)
                                       .FirstOrDefaultAsync();
            aplikacja.Preferencje = await _context.Preferencje
                                   .Where(pref => pref.IdAplikacji == aplikacja.Id)
                                   .OrderBy(pref => pref.Priorytet)
                                   .ToListAsync();
            
            foreach (var preferencja in aplikacja.Preferencje)
            {
                preferencja.Kierunek = await _context.Kierunki.FirstOrDefaultAsync(k => k.Id == preferencja.IdKierunku);
                if (preferencja.IdWybranejSpecjalizacji != null)
                {
                    preferencja.WybranaSpecjalizacja = await _context.Specjalizacje.FirstOrDefaultAsync(s => s.Id == preferencja.IdWybranejSpecjalizacji);
                }
            }
            return View(aplikacja);
            
        }
        // POST: Kandydaci/ReorderPrefs
        [HttpPost]
        public IActionResult ReorderPrefs([FromBody] ReorderRequest request)
        {
            Aplikacja aplikacja =  _context.Aplikacje.FirstOrDefault(a => a.Id == request.IdAplikacji);
            List<Preferencja> preferencje = _context.Preferencje
                .Where(pref => pref.IdAplikacji == request.IdAplikacji)
                .ToList();

            Dictionary<Preferencja, int> NowePreferencje = new Dictionary<Preferencja, int>();

            foreach (var pref in request.Priorytety.Keys)
            {
                Preferencja Pref = preferencje.Where(p => p.Priorytet == pref).First();
                NowePreferencje.Add(Pref, request.Priorytety[pref]);
            }
            foreach (var pref in NowePreferencje.Keys)
            {
                pref.Priorytet = NowePreferencje[pref];
                _context.Preferencje.Update(pref);

            }
            _context.SaveChanges();
            return Ok();
        }

        // DELETE: Kandydaci/DeleteApplication/{id}
        [HttpDelete]
        public async Task<IActionResult> DeleteApplication(long id)
        {
            Aplikacja aplikacja = await _context.Aplikacje.FindAsync(id);
            if (aplikacja == null)
            {
                return NotFound();
            }
            _context.Aplikacje.Remove(aplikacja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Aplikacja));
        }
    }
}
