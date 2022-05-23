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

        /// <summary>
        /// Returns all packages.
        /// </summary>
        /// <param name="orderParam">Deside how to order the packages.</param>
        public async Task<IActionResult> Index(string orderParam)
        {
            // Set which orderParam that should be for each header. Makes it possible to toggle between desc and asc order.
            ViewBag.ExpiryDateOrderParam = String.IsNullOrEmpty(orderParam) ? "expiryDateDesc" : "";
            ViewBag.PackingDateOrderParam = orderParam == "packingDate" ? "packingDateDesc" : "packingDate";
            ViewBag.WeightOrderParam = orderParam == "weight" ? "weightDesc" : "weight";

            IQueryable<Package> packages = _context.Package
                .Include(p => p.Carcass)
                .ThenInclude(c => c.Specie)
                .Include(p => p.PrimalCut)
                .Include(p => p.StoragePlace);

            // Let the databse order the packages.
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

        /// <summary>
        /// Returns detailed info about a specific package.
        /// </summary>
        /// <param name="id">The id of the wanted package.</param>
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

        /// <summary>
        /// Returns a form to create a new package.
        /// </summary>
        public IActionResult Create()
        {
            PopulateSelectList();
            return View();
        }

        /// <summary>
        /// Creates a new package in the database.
        /// </summary>
        /// <param name="package">The package to create.</param>
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

        /// <summary>
        /// Returns a prefilled form for the wanted package. 
        /// </summary>
        /// <param name="id">The id of the wanted package.</param>
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

        /// <summary>
        /// Updates a package.
        /// </summary>
        /// <param name="id">Id of the package to update.</param>
        /// <param name="package">The new data.</param>
        /// 
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

        /// <summary>
        /// Returns a confrimation to delete.
        /// </summary>
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

        /// <summary>
        /// Removes the package.
        /// </summary>
        /// <param name="id">Id to remove.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var package = await _context.Package.FindAsync(id);
            _context.Package.Remove(package);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// True if the package exists.
        /// </summary>
        /// <param name="id">The id to check.</param>
        private bool PackageExists(int id)
        {
            return _context.Package.Any(e => e.Id == id);
        }

        /// <summary>
        /// Prepare empty select lists.
        /// </summary>
        private void PopulateSelectList()
        {
            PopulateSelectList(null, null, null);
        }

        /// <summary>
        /// Prepare select lists with options.
        /// </summary>
        private void PopulateSelectList(int? selectedCarcass, int? selectedPrimalCut, int? selectedStoragePlace)
        {
            ViewBag.Carcasses = new SelectList(_context.Carcass.Include(c => c.Specie).OrderByDescending(c => c.ShotDate), "Id", "IdentifyingName", selectedCarcass);
            ViewBag.PrimalCuts = new SelectList(_context.PrimalCut, "Id", "IdentifyingName", selectedPrimalCut);
            ViewBag.StoragePlaces = new SelectList(_context.StoragePlace, "Id", "IdentifyingName", selectedStoragePlace);
        }
    }
}
