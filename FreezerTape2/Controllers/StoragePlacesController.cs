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
    public class StoragePlacesController : Controller
    {
        private readonly FreezerContext _context;

        public StoragePlacesController(FreezerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all storage places. GET: /storageplaces
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var storagePlaces = await _context.StoragePlace
                .Include(s => s.Packages)
                .ToListAsync();
            return View(storagePlaces);
        }

        /// <summary>
        /// Returns a specific storage place. GET: /storageplaces/details/5
        /// </summary>
        /// <param name="id">The id of the wanted storage place.</param>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Make sure that the necessary data is included.
            var storagePlace = await _context.StoragePlace
                .Include(s => s.Packages)
                .ThenInclude(p => p.Carcass)
                .ThenInclude(c => c.Specie)
                .Include(s => s.Packages)
                .ThenInclude(p => p.PrimalCut)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storagePlace == null)
            {
                return NotFound();
            }

            return View(storagePlace);
        }

        /// <summary>
        /// Returns a form to create a new storage place. GET: /storageplaces/create
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new storage palce. POST: /storageplaces/create
        /// </summary>
        /// <param name="storagePlace">The storage palce to create.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] StoragePlace storagePlace)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storagePlace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storagePlace);
        }

        /// <summary>
        /// Returns a prefilled form for the wanted storage place. GET: /storageplaces/edit/5
        /// </summary>
        /// <param name="id">The id of the wanted storage place.</param>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storagePlace = await _context.StoragePlace.FindAsync(id);
            if (storagePlace == null)
            {
                return NotFound();
            }
            return View(storagePlace);
        }

        /// <summary>
        /// Updates a storage place.
        /// </summary>
        /// <param name="id">Id of the storage place to update.</param>
        /// <param name="storagePlace">The new data.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] StoragePlace storagePlace)
        {
            if (id != storagePlace.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storagePlace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoragePlaceExists(storagePlace.Id))
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
            return View(storagePlace);
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

            var storagePlace = await _context.StoragePlace
                .Include(s => s.Packages)
                .ThenInclude(p => p.Carcass)
                .ThenInclude(c => c.Specie)
                .Include(s => s.Packages)
                .ThenInclude(p => p.PrimalCut)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storagePlace == null)
            {
                return NotFound();
            }

            return View(storagePlace);
        }

        /// <summary>
        /// Removes the storgae place.
        /// </summary>
        /// <param name="id">Id to remove.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storagePlace = await _context.StoragePlace.FindAsync(id);
            _context.StoragePlace.Remove(storagePlace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// True if the storgae place exists.
        /// </summary>
        /// <param name="id">The id to check.</param>
        private bool StoragePlaceExists(int id)
        {
            return _context.StoragePlace.Any(e => e.Id == id);
        }
    }
}
