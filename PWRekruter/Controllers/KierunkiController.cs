﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWRekruter.Models;
using PWRekruter.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;
using PWRekruter.Data;

namespace PWRekruter.Controllers
{
    public class KierunkiController : Controller
    {
        private readonly PWRekruterDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public KierunkiController(PWRekruterDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Kierunki
        public IActionResult Index(string nazwa, string wydzial, StopienStudiow? stopien, 
            FormaStudiow? forma, string dyscyplina)
        {
            
            var kierunki = _context.Kierunki
                .Where(k => string.IsNullOrEmpty(nazwa) || k.Nazwa.ToLower().Contains(nazwa.ToLower()))
                .Where(k => string.IsNullOrEmpty(wydzial) || k.SymbolWydzialu == wydzial)
                .Where(k => !stopien.HasValue || k.Stopien == stopien)
                .Where(k => !forma.HasValue || k.Forma == forma)
                .Where(k => string.IsNullOrEmpty(dyscyplina) || k.DyscyplinaNaukowa ==dyscyplina)
                .ToList();

            var wydzialy = _context.Wydzialy.ToList();

            var dyscypliny = _context.Kierunki
                .Select(k => k.DyscyplinaNaukowa)
                .Distinct()
                .ToList();

            ViewBag.Dyscypliny = dyscypliny;
            ViewBag.Wydzialy = wydzialy;

            return View(kierunki);
        }

        public async Task<IActionResult> PobierzProgramStudiow(long id)
        {
            var programStudiow = await _context.ProgramyStudiow.FirstOrDefaultAsync(p => p.Id == id);

            if (programStudiow == null)
            {
                return NotFound();
            }

            return DownloadFile(programStudiow.ProgramSciezka);
        }

        public  IActionResult PobierzPlanStudiow(long id)
        {
            var programStudiow = _context.ProgramyStudiow.FirstOrDefault(p => p.Id == id);

            if (programStudiow == null)
            {
                return NotFound();
            }

            return DownloadFile(programStudiow.PlanSciezka);
        }

        private PhysicalFileResult DownloadFile(string path)
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, path);

            var contentType = "APPLICATION/OCTET-STREAM";
            var fileName = Path.GetFileName(filePath);

            return PhysicalFile(filePath, contentType, fileName);
        }



        // GET: Kierunki/Details/5
        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kierunek = _context.Kierunki
                .Include(k => k.HistoryczneProgi)
                .FirstOrDefault(m => m.Id == id);

            if (kierunek == null)
            {
                return NotFound();
            }

            return View(kierunek);
        }

        

        // GET: Kierunki/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kierunek = await _context.Kierunki.FindAsync(id);
            if (kierunek == null)
            {
                return NotFound();
            }
            return View(kierunek);
        }

        // POST: Kierunki/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Skrot,Nazwa,Stopien,Tryb,Forma,CzasTrwania,DyscyplinaNaukowa,JezykWykladowy,Profil,Czesne,CzesneDlaCudzoziemcow,LiczbaMiejsc,OplataRekrutacyjna,Opis")] Kierunek kierunek)
        {
            if (id != kierunek.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kierunek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KierunekExists(kierunek.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kierunek);
        }

        // GET: Kierunki/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kierunek = await _context.Kierunki
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kierunek == null)
            {
                return NotFound();
            }

            return View(kierunek);
        }

        // POST: Kierunki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var kierunek = await _context.Kierunki.FindAsync(id);
            _context.Kierunki.Remove(kierunek);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KierunekExists(long id)
        {
            return _context.Kierunki.Any(e => e.Id == id);
        }
        public IActionResult IndexAdm(string Symbol, string? SearchString)
        {
            ViewBag.Symbol = Symbol;

            var kierunki = _context.Kierunki.Where(k => k.SymbolWydzialu == Symbol);

            if (!string.IsNullOrEmpty(SearchString))
            {
				kierunki = kierunki.Where(k => k.Nazwa.ToLower().Contains(SearchString.ToLower()));
            }
            return View(kierunki.ToList());
        }

        // GET: Kierunki/Create
        public IActionResult Create(string Symbol)
        {
            ViewBag.Symbol = Symbol;
            ViewBag.ProgramyStudiow = getProgramSelectList();

			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Create(Kierunek kierunek)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(kierunek);
                if (kierunek.Specjalizacje != null)
                {
                    foreach (Specjalizacja spec in kierunek.Specjalizacje)
                    {
                        _context.Add(spec);
                    }
                }
                
                _context.SaveChanges();
                return RedirectToAction(nameof(IndexAdm), new { symbol = kierunek.SymbolWydzialu });
            }
            ViewBag.Symbol = kierunek.SymbolWydzialu;
            ViewBag.ProgramyStudiow = getProgramSelectList();

            return View(kierunek);
        }


        private List<SelectListItem> getProgramSelectList()
        {
            List<SelectListItem> programyStudiow = new List<SelectListItem>
            {
                new SelectListItem { Text = "Wybierz opcję", Value = string.Empty }
            };
            programyStudiow.AddRange(_context.ProgramyStudiow.Select(p => new SelectListItem
            {
                Text = p.Nazwa,
                Value = p.Id.ToString()
            }).ToList());

            return programyStudiow;
        }
    }
}
