#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreezerTape2.Data;
using FreezerTape2.Models;

namespace FreezerTape2.Controllers
{
    public class SpeciesController : Controller
    {
        private readonly FreezerContext _context;

        public SpeciesController(FreezerContext context)
        {
            _context = context;
        }

        // GET: Species
        public async Task<IActionResult> Index()
        {
            var species = await _context.Specie
                .Include(s => s.PrimalCuts)
                .Include(s => s.Carcasses)
                .ThenInclude(c => c.Packages)
                .ToListAsync();
            return View(species);
        }

        // GET: Species/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specie = await _context.Specie
                .Include(s => s.PrimalCuts)
                .Include(s => s.Carcasses)
                .ThenInclude(c => c.Packages)
                .ThenInclude(p => p.StoragePlace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specie == null)
            {
                return NotFound();
            }

            return View(specie);
        }

        // GET: Species/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Species/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ShelfLife")] Specie specie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specie);
        }

        // GET: Species/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specie = await _context.Specie.FindAsync(id);
            if (specie == null)
            {
                return NotFound();
            }
            return View(specie);
        }

        // POST: Species/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ShelfLife")] Specie specie)
        {
            if (id != specie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecieExists(specie.Id))
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
            return View(specie);
        }

        // GET: Species/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specie = await _context.Specie
                .Include(s => s.PrimalCuts)
                .ThenInclude(p => p.Packages)
                .ThenInclude(p => p.StoragePlace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (specie == null)
            {
                return NotFound();
            }

            return View(specie);
        }

        // POST: Species/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specie = await _context.Specie.FindAsync(id);
            _context.Specie.Remove(specie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecieExists(int id)
        {
            return _context.Specie.Any(e => e.Id == id);
        }
    }
}
