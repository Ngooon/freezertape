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
    public class PackagesController : Controller
    {
        private readonly FreezerContext _context;

        public PackagesController(FreezerContext context)
        {
            _context = context;
        }

        // GET: Packages
        public async Task<IActionResult> Index()
        {
            var freezerContext = _context.Package.Include(p => p.Carcass).Include(p => p.PrimalCut).Include(p => p.StoragePlace);
            return View(await freezerContext.ToListAsync());
        }

        // GET: Packages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package
                .Include(p => p.Carcass)
                .Include(p => p.PrimalCut)
                .Include(p => p.StoragePlace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // GET: Packages/Create
        public IActionResult Create()
        {
            ViewData["CarcassId"] = new SelectList(_context.Carcass, "Id", "Id");
            ViewData["PrimalCutId"] = new SelectList(_context.Set<PrimalCut>(), "Id", "Id");
            ViewData["StoragePlaceId"] = new SelectList(_context.Set<StoragePlace>(), "Id", "Id");
            return View();
        }

        // POST: Packages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Weight,PackingDate,ExpiryDate,Comment,CarcassId,PrimalCutId,StoragePlaceId")] Package package)
        {
            if (ModelState.IsValid)
            {
                _context.Add(package);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarcassId"] = new SelectList(_context.Carcass, "Id", "Id", package.CarcassId);
            ViewData["PrimalCutId"] = new SelectList(_context.Set<PrimalCut>(), "Id", "Id", package.PrimalCutId);
            ViewData["StoragePlaceId"] = new SelectList(_context.Set<StoragePlace>(), "Id", "Id", package.StoragePlaceId);
            return View(package);
        }

        // GET: Packages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            ViewData["CarcassId"] = new SelectList(_context.Carcass, "Id", "Id", package.CarcassId);
            ViewData["PrimalCutId"] = new SelectList(_context.Set<PrimalCut>(), "Id", "Id", package.PrimalCutId);
            ViewData["StoragePlaceId"] = new SelectList(_context.Set<StoragePlace>(), "Id", "Id", package.StoragePlaceId);
            return View(package);
        }

        // POST: Packages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Weight,PackingDate,ExpiryDate,Comment,CarcassId,PrimalCutId,StoragePlaceId")] Package package)
        {
            if (id != package.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(package);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PackageExists(package.Id))
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
            ViewData["CarcassId"] = new SelectList(_context.Carcass, "Id", "Id", package.CarcassId);
            ViewData["PrimalCutId"] = new SelectList(_context.Set<PrimalCut>(), "Id", "Id", package.PrimalCutId);
            ViewData["StoragePlaceId"] = new SelectList(_context.Set<StoragePlace>(), "Id", "Id", package.StoragePlaceId);
            return View(package);
        }

        // GET: Packages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var package = await _context.Package
                .Include(p => p.Carcass)
                .Include(p => p.PrimalCut)
                .Include(p => p.StoragePlace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        // POST: Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var package = await _context.Package.FindAsync(id);
            _context.Package.Remove(package);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PackageExists(int id)
        {
            return _context.Package.Any(e => e.Id == id);
        }
    }
}
