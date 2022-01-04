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
        public async Task<IActionResult> Index(string orderParam)
        {
            ViewBag.ExpiryDateOrderParam = String.IsNullOrEmpty(orderParam) ? "expiryDateDesc" : "";
            ViewBag.PackingDateOrderParam = orderParam == "packingDate" ? "packingDateDesc" : "packingDate";
            ViewBag.WeightOrderParam = orderParam == "weight" ? "weightDesc" : "weight";

            IQueryable<Package> packages = _context.Package
                .Include(p => p.Carcass)
                .ThenInclude(c => c.Specie)
                .Include(p => p.PrimalCut)
                .Include(p => p.StoragePlace);

            switch (orderParam)
            {
                case "expiryDateDesc":
                    packages = packages.OrderByDescending(p => p.ExpiryDate);
                    break;
                case "packingDate":
                    packages = packages.OrderBy(p => p.PackingDate);
                    break;
                case "packingDateDesc":
                    packages = packages.OrderByDescending(p => p.PackingDate);
                    break;
                case "weight":
                    packages = packages.OrderBy(p => p.Weight);
                    break;
                case "weightDesc":
                    packages = packages.OrderByDescending(p => p.Weight);
                    break;
                default:
                    packages = packages.OrderBy(p => p.ExpiryDate);
                    break;
            }

            return View(packages);
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
                .ThenInclude(c => c.Specie)
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
            PopulateSelectList();
            return View();
        }

        // POST: Packages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Weight,PackingDate,ExpiryDate,Comment,CarcassId,PrimalCutId,StoragePlaceId")] Package package)
        {
            if (package.ExpiryDate == null)
            {
                var carcass = await _context.Carcass.Include(c => c.Specie).FirstOrDefaultAsync(c => c.Id == package.CarcassId);
                int shelfLifeInMonths = carcass.Specie.ShelfLife.GetValueOrDefault();
                DateTime startTime = package.PackingDate != null ? package.PackingDate.GetValueOrDefault() : DateTime.Now.Date;
                package.ExpiryDate = startTime.AddMonths(shelfLifeInMonths);
            }

            if (ModelState.IsValid)
            {
                _context.Add(package);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateSelectList(package.CarcassId, package.PrimalCutId, package.StoragePlaceId);
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

            PopulateSelectList(package.CarcassId, package.PrimalCutId, package.StoragePlaceId);
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
            PopulateSelectList(package.CarcassId, package.PrimalCutId, package.StoragePlaceId);
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
                .ThenInclude(c => c.Specie)
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

        private void PopulateSelectList()
        {
            PopulateSelectList(null, null, null);
        }

        private void PopulateSelectList(int? selectedCarcass, int? selectedPrimalCut, int? selectedStoragePlace)
        {
            ViewBag.Carcasses = new SelectList(_context.Carcass.Include(c => c.Specie).OrderByDescending(c => c.ShotDate), "Id", "IdentifyingName", selectedCarcass);
            ViewBag.PrimalCuts = new SelectList(_context.PrimalCut, "Id", "IdentifyingName", selectedPrimalCut);
            ViewBag.StoragePlaces = new SelectList(_context.StoragePlace, "Id", "IdentifyingName", selectedStoragePlace);
        }
    }
}
