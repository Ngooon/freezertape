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
    public class PrimalCutsController : Controller
    {
        private readonly FreezerContext _context;

        public PrimalCutsController(FreezerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all primal cuts.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var primalCuts = await _context.PrimalCut
                .Include(p => p.Packages)
                .Include(p => p.Species)
                .ToListAsync();
            return View(primalCuts);
        }

        /// <summary>
        /// Returns detailed info about a specific primal cut.
        /// </summary>
        /// <param name="id">The id of the wanted primal cut.</param>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var primalCut = await _context.PrimalCut
                .Include(primalCut => primalCut.Packages)
                .ThenInclude(package => package.Carcass)
                .ThenInclude(carcass => carcass.Specie)
                .Include(primalCut => primalCut.Packages)
                .ThenInclude(package => package.StoragePlace)
                .Include(primalCut => primalCut.Species)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (primalCut == null)
            {
                return NotFound();
            }

            return View(primalCut);
        }

        /// <summary>
        /// Returns a form to create a new primal cut.
        /// </summary>
        public IActionResult Create()
        {
            PopulateSelectList();
            return View();
        }

        /// <summary>
        /// Creates a new primal cut in the database.
        /// </summary>
        /// <param name="primalCut">The primal cut to create.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] PrimalCut primalCut, [FromForm] int[] specieIds)
        {
            primalCut.Species = await _context.Specie.Where(i => specieIds.Contains(i.Id)).ToListAsync();

            if (ModelState.IsValid)
            {
                _context.Add(primalCut);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateSelectList(specieIds);
            return View(primalCut);
        }

        /// <summary>
        /// Returns a prefilled form for the wanted primal cut. 
        /// </summary>
        /// <param name="id">The id of the wanted primal cut.</param>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var primalCut = await _context.PrimalCut
                .Include(primalCut => primalCut.Packages)
                .Include(primalCut => primalCut.Species)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (primalCut == null)
            {
                return NotFound();
            }

            int[] selectedSpecies = primalCut.Species.Select(c => c.Id).ToArray();
            PopulateSelectList(selectedSpecies);

            return View(primalCut);
        }

        /// <summary>
        /// Updates a primal cut.
        /// </summary>
        /// <param name="id">Id of the primal cut to update.</param>
        /// <param name="primalCut">The new data.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PrimalCut primalCut, [FromForm] int[] species)
        {
            if (id != primalCut.Id)
            {
                return NotFound();
            }

            primalCut.Species = await _context.Specie.Where(i => species.Contains(i.Id)).ToListAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    var primalCutToUpdate = await _context.PrimalCut
                        .Include(primalCut => primalCut.Species)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    primalCutToUpdate.Copy(primalCut);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrimalCutExists(primalCut.Id))
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
            int[] selectedSpecies = primalCut.Species.Select(c => c.Id).ToArray();
            PopulateSelectList(selectedSpecies);
            return View(primalCut);
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

            var primalCut = await _context.PrimalCut
                .Include(primalCut => primalCut.Packages)
                .ThenInclude(package => package.Carcass)
                .ThenInclude(carcass => carcass.Specie)
                .Include(primalCut => primalCut.Packages)
                .ThenInclude(package => package.StoragePlace)
                .Include(primalCut => primalCut.Species)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (primalCut == null)
            {
                return NotFound();
            }

            return View(primalCut);
        }

        /// <summary>
        /// Removes the primal cut.
        /// </summary>
        /// <param name="id">Id to remove.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var primalCut = await _context.PrimalCut.FindAsync(id);
            _context.PrimalCut.Remove(primalCut);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// True if the primal cut exists.
        /// </summary>
        /// <param name="id">The id to check.</param>
        private bool PrimalCutExists(int id)
        {
            return _context.PrimalCut.Any(e => e.Id == id);
        }

        /// <summary>
        /// Prepare a empty select list.
        /// </summary>
        private void PopulateSelectList()
        {
            PopulateSelectList(null);
        }

        /// <summary>
        /// Prepare a select list with options.
        /// </summary>
        private void PopulateSelectList(int[] selectedSpecie)
        {
            ViewData["Species"] = new MultiSelectList(_context.Specie, "Id", "IdentifyingName", selectedSpecie);
        }
    }
}
