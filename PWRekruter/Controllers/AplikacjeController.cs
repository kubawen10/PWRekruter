using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWRekruter.Data;
using PWRekruter.Enums;
using PWRekruter.Models;

namespace PWRekruter.Controllers
{
    public class AplikacjeController : Controller
    {
        private readonly PWRekruterDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AplikacjeController(PWRekruterDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Aplikacje
        public async Task<IActionResult> Index(long? id, string kandydat, string wydzial, long? kierunek, 
            StatusAplikacji? status)
        {
            var wydzialy = await _context.Wydzialy.ToListAsync();
            var kierunki = await _context.Kierunki.ToListAsync();
            ViewBag.Wydzialy = wydzialy;
            ViewBag.Kierunki = kierunki;
            /*
            if(id==null && string.IsNullOrEmpty(kandydat) && string.IsNullOrEmpty(wydzial) && kierunek==null
                && !status.HasValue)
            {
                return View(new List<Aplikacja>());
            }*/

            var aplikacje = _context.Aplikacje.Include(a => a.Kandydat)
                    .Include(a => a.Preferencje).ThenInclude(p => p.Kierunek)
                    .Where(a => id==null || a.Id==id)
                    .Where(a => string.IsNullOrEmpty(kandydat) ||
                        (a.Kandydat.Imie+" "+a.Kandydat.Nazwisko).Contains(kandydat))
                    .Where(a => string.IsNullOrEmpty(wydzial) ||
                        a.Preferencje.Any(p => p.Kierunek.Wydzial.Symbol == wydzial))
                    .Where(a => kierunek==null ||
                        a.Preferencje.Any(p => p.Kierunek.Id == kierunek))
                    .Where(a => !status.HasValue || a.Status==status);
           
            return View(await aplikacje.ToListAsync());
        }

        // GET: Aplikacje/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aplikacja = await _context.Aplikacje
                .Include(a => a.Kandydat)
                .Include(a => a.Dokumenty)
                .Include(a => a.Preferencje)
                .ThenInclude(p => p.Kierunek)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aplikacja == null)
            {
                return NotFound();
            }

            return View(aplikacja);
        }

        public async Task<IActionResult> DownloadDocument(long id)
        {
            var dokument = await _context.Dokumenty.FirstOrDefaultAsync(d => d.Id == id);

            if (dokument == null)
            {
                return NotFound();
            }

            return DownloadFile(dokument.SciezkaPliku);
        }

        private PhysicalFileResult DownloadFile(string path)
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, path);

            var contentType = "APPLICATION/OCTET-STREAM";
            var fileName = Path.GetFileName(filePath);

            return PhysicalFile(filePath, contentType, fileName);
        }

        // GET: Aplikacje/Create
        public IActionResult Create()
        {
            ViewData["IdKandydata"] = new SelectList(_context.Kandydaci, "Id", "Email");
            return View();
        }

        // POST: Aplikacje/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,Oplacona,DataZlozenia,IdKandydata")] Aplikacja aplikacja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aplikacja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdKandydata"] = new SelectList(_context.Kandydaci, "Id", "Email", aplikacja.IdKandydata);
            return View(aplikacja);
        }

        // GET: Aplikacje/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aplikacja = await _context.Aplikacje.FindAsync(id);
            if (aplikacja == null)
            {
                return NotFound();
            }
            ViewData["IdKandydata"] = new SelectList(_context.Kandydaci, "Id", "Email", aplikacja.IdKandydata);
            return View(aplikacja);
        }

        // POST: Aplikacje/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Status,Oplacona,DataZlozenia,IdKandydata")] Aplikacja aplikacja)
        {
            if (id != aplikacja.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aplikacja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AplikacjaExists(aplikacja.Id))
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
            ViewData["IdKandydata"] = new SelectList(_context.Kandydaci, "Id", "Email", aplikacja.IdKandydata);
            return View(aplikacja);
        }

        // GET: Aplikacje/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aplikacja = await _context.Aplikacje
                .Include(a => a.Kandydat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aplikacja == null)
            {
                return NotFound();
            }

            return View(aplikacja);
        }

        // POST: Aplikacje/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var aplikacja = await _context.Aplikacje.FindAsync(id);
            _context.Aplikacje.Remove(aplikacja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AplikacjaExists(long id)
        {
            return _context.Aplikacje.Any(e => e.Id == id);
        }
    }
}
