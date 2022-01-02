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

        // GET: StoragePlaces
        public async Task<IActionResult> Index()
        {
            return View(await _context.StoragePlace.ToListAsync());
        }

        // GET: StoragePlaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storagePlace = await _context.StoragePlace
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storagePlace == null)
            {
                return NotFound();
            }

            return View(storagePlace);
        }

        // GET: StoragePlaces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StoragePlaces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: StoragePlaces/Edit/5
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

        // POST: StoragePlaces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: StoragePlaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storagePlace = await _context.StoragePlace
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storagePlace == null)
            {
                return NotFound();
            }

            return View(storagePlace);
        }

        // POST: StoragePlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storagePlace = await _context.StoragePlace.FindAsync(id);
            _context.StoragePlace.Remove(storagePlace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoragePlaceExists(int id)
        {
            return _context.StoragePlace.Any(e => e.Id == id);
        }
    }
}
