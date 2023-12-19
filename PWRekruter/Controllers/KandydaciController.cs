using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PWRekruter.Data;
using PWRekruter.Models;
using PWRekruter.Services;
using System.Linq;
using System.Threading.Tasks;

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
            return View(await _context.Kandydaci.FirstOrDefaultAsync(k => k.Id == kandydatId));
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kandydat = await _context.Kandydaci.FindAsync(id);
            if (kandydat == null)
            {
                return NotFound();
            }
            return View(kandydat);
        }

        // POST: Kandydaci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Imie,DrugieImie,Nazwisko,Pesel,Plec,DataUrodzenia,Panstwo,KodPocztowy,Miejscowosc,Ulica,NumerBudynku,NumerMieszkania,Id,Email,Haslo")] Kandydat kandydat)
        {
            if (id != kandydat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kandydat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KandydatExists(kandydat.Id))
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
            return View(kandydat);
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
    }
}
