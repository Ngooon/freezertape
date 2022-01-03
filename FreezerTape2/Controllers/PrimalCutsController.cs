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

        // GET: PrimalCuts
        public async Task<IActionResult> Index()
        {
            var primalCuts = await _context.PrimalCut
                .Include(p => p.Packages)
                .Include(p => p.Species)
                .ToListAsync();
            return View(primalCuts);
        }

        // GET: PrimalCuts/Details/5
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

        // GET: PrimalCuts/Create
        public IActionResult Create()
        {
            PopulateSelectList();
            return View();
        }

        // POST: PrimalCuts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: PrimalCuts/Edit/5
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

        // POST: PrimalCuts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: PrimalCuts/Delete/5
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

        // POST: PrimalCuts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var primalCut = await _context.PrimalCut.FindAsync(id);
            _context.PrimalCut.Remove(primalCut);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrimalCutExists(int id)
        {
            return _context.PrimalCut.Any(e => e.Id == id);
        }

        //ViewBag[]"Species"] = new MultiSelectList(_context.Specie.ToList(), "Id", "Name", selectedSpecie);
        private void PopulateSelectList()
        {
            PopulateSelectList(null);
        }

        private void PopulateSelectList(int[] selectedSpecie)
        {
            ViewData["Species"] = new MultiSelectList(_context.Specie, "Id", "IdentifyingName", selectedSpecie);
        }
    }
}
