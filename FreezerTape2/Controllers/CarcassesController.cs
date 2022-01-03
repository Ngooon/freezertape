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

        // GET: Carcasses
        public async Task<IActionResult> Index()
        {
            var carcasses = await _context.Carcass
                .Include(c => c.Specie)
                .Include(c => c.Packages)
                .ToListAsync();
            return View(carcasses);
        }

        // GET: Carcasses/Details/5
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

        // GET: Carcasses/Create
        public IActionResult Create()
        {
            PopulateSelectList();
            return View();
        }

        // POST: Carcasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Carcasses/Edit/5
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

        // POST: Carcasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Carcasses/Delete/5
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

        // POST: Carcasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carcass = await _context.Carcass.FindAsync(id);
            _context.Carcass.Remove(carcass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarcassExists(int id)
        {
            return _context.Carcass.Any(e => e.Id == id);
        }

        private void PopulateSelectList()
        {
            PopulateSelectList(null);
        }

        private void PopulateSelectList(int? selectedSpecie)
        {
            ViewData["Species"] = new SelectList(_context.Specie, "Id", "IdentifyingName", selectedSpecie);
        }
    }
}
