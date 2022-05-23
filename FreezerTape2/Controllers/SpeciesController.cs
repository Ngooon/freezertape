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

        /// <summary>
        /// Returns all species.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var species = await _context.Specie
                .Include(s => s.PrimalCuts)
                .Include(s => s.Carcasses)
                .ThenInclude(c => c.Packages)
                .ToListAsync();
            return View(species);
        }

        /// <summary>
        /// Returns detailed info about a specific specie.
        /// </summary>
        /// <param name="id">The id of the wanted specie.</param>
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

        /// <summary>
        /// Returns a form to create a new specie.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new specie in the database.
        /// </summary>
        /// <param name="specie">The specie to create.</param>
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

        /// <summary>
        /// Returns a prefilled form for the wanted specie. 
        /// </summary>
        /// <param name="id">The id of the wanted specie.</param>
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

        /// <summary>
        /// Updates a specie.
        /// </summary>
        /// <param name="id">Id of the specie to update.</param>
        /// <param name="specie">The new data.</param>

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

        /// <summary>
        /// Returns a confrimation to delete.
        /// </summary>
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

        /// <summary>
        /// Removes the specie.
        /// </summary>
        /// <param name="id">Id to remove.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specie = await _context.Specie.FindAsync(id);
            _context.Specie.Remove(specie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// True if the specie exists.
        /// </summary>
        /// <param name="id">The id to check.</param>
        private bool SpecieExists(int id)
        {
            return _context.Specie.Any(e => e.Id == id);
        }
    }
}
