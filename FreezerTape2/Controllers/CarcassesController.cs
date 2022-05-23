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
    public class CarcassesController : Controller
    {
        private readonly FreezerContext _context;

        public CarcassesController(FreezerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all carcasses.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var carcasses = await _context.Carcass
                .Include(c => c.Specie)
                .Include(c => c.Packages)
                .ToListAsync();
            return View(carcasses);
        }

        /// <summary>
        /// Returns detailed info about a specific carcass.
        /// </summary>
        /// <param name="id">The id of the wanted carcass.</param>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carcass = await _context.Carcass
                .Include(c => c.Specie)
                .Include(c => c.Packages)
                .ThenInclude(p => p.PrimalCut)
                .Include(c => c.Packages)
                .ThenInclude(p => p.StoragePlace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carcass == null)
            {
                return NotFound();
            }

            return View(carcass);
        }

        /// <summary>
        /// Returns a form to create a new carcass.
        /// </summary>
        public IActionResult Create()
        {
            PopulateSelectList();
            return View();
        }

        /// <summary>
        /// Creates a new carcass in the database.
        /// </summary>
        /// <param name="carcass">The carcass to create.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShotDate,ShotPlace,ShotBy,LiveWeight,DressedWeight,PositionOfBulkhead,Gender,Age,Comment,SpecieId")] Carcass carcass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carcass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateSelectList(carcass.SpecieId);
            return View(carcass);
        }

        /// <summary>
        /// Returns a prefilled form for the wanted carcass. 
        /// </summary>
        /// <param name="id">The id of the wanted carcass.</param>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carcass = await _context.Carcass.FindAsync(id);
            if (carcass == null)
            {
                return NotFound();
            }
            PopulateSelectList(carcass.SpecieId);
            return View(carcass);
        }

        /// <summary>
        /// Updates a carcass.
        /// </summary>
        /// <param name="id">Id of the carcass to update.</param>
        /// <param name="carcass">The new data.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShotDate,ShotPlace,ShotBy,LiveWeight,DressedWeight,PositionOfBulkhead,Gender,Age,Comment,SpecieId")] Carcass carcass)
        {
            if (id != carcass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carcass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarcassExists(carcass.Id))
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
            PopulateSelectList(carcass.SpecieId);
            return View(carcass);
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

            var carcass = await _context.Carcass
                .Include(c => c.Specie)
                .Include(c => c.Packages)
                .ThenInclude(p => p.PrimalCut)
                .Include(c => c.Packages)
                .ThenInclude(p => p.StoragePlace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carcass == null)
            {
                return NotFound();
            }

            return View(carcass);
        }

        /// <summary>
        /// Removes the carcass.
        /// </summary>
        /// <param name="id">Id to remove.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carcass = await _context.Carcass.FindAsync(id);
            _context.Carcass.Remove(carcass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// True if the carcass exists.
        /// </summary>
        /// <param name="id">The id to check.</param>
        private bool CarcassExists(int id)
        {
            return _context.Carcass.Any(e => e.Id == id);
        }

        /// <summary>
        /// Prepare empty select lists.
        /// </summary>
        private void PopulateSelectList()
        {
            PopulateSelectList(null);
        }

        /// <summary>
        /// Prepare select lists with options.
        /// </summary>
        private void PopulateSelectList(int? selectedSpecie)
        {
            ViewData["Species"] = new SelectList(_context.Specie, "Id", "IdentifyingName", selectedSpecie);
        }
    }
}
